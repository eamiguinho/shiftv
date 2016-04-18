using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Autofac;
using Shiftv.Common;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.Data.JsonTrakt;
using Shiftv.Contracts.Services.PlatformServices;
using Shiftv.DataModel;
using Shiftv.Global;
using Shiftv.Views.Shows.Player;

namespace Shiftv.ViewModels.OfflineContent
{
    public class OfflineContentManagerViewModel : ViewModelBase
    {
        private List<DownloadOperation> activeDownloads;
        private CancellationTokenSource cts;
        private ObservableCollection<DownloadEpisodeStatus> _downloads;
        private ObservableCollection<EpisodeDataModel> _downloadedEpisodes;
        private IDownloadService _downloadService;
        private RelayCommand _setDownloadPause;
        private RelayCommand _resumeDownloadCommand;
        private RelayCommand _cancelDownloadCommand;
        private RelayCommand<EpisodeDataModel> _downloadedClicked;
        private RelayCommand _deleteDownloadedCommand;

        public OfflineContentManagerViewModel()
        {
            _downloadService = Ioc.Container.Resolve<IDownloadService>();
        }

        public ObservableCollection<DownloadEpisodeStatus> Downloads
        {
            get { return _downloads ?? (_downloads = new ObservableCollection<DownloadEpisodeStatus>()); }
        }

        public ObservableCollection<EpisodeDataModel> DownloadedEpisodes
        {
            get { return _downloadedEpisodes ?? (_downloadedEpisodes = new ObservableCollection<EpisodeDataModel>()); }
        }

        public bool CanPauseDownload
        {
            get { return Downloads.Any(x=>x.IsSelected && !x.IsPauseByUser && !x.IsInternetDown); }
        }

        public bool CanResumeDownload
        {
            get { return Downloads.Any(x => x.IsSelected && x.IsPauseByUser && !x.IsInternetDown); }
        }

        public RelayCommand SetDownloadPause
        {
            get { return _setDownloadPause ?? (_setDownloadPause = new RelayCommand(DownloadPause)); }
        } 
        
        public RelayCommand ResumeDownloadCommand
        {
            get { return _resumeDownloadCommand ?? (_resumeDownloadCommand = new RelayCommand(ResumeDownload)); }
        }

        public bool IsAppbarOpen
        {
            get
            {
                return Downloads.Count(x=>x.IsSelected) != 0 || SelectedDownloaded != null;
            }
        }

        public bool CanCancelDownload
        {
            get { return Downloads.Any(x => x.IsSelected); }
        }

        public RelayCommand CancelDownloadCommand
        {
            get { return _cancelDownloadCommand ?? (_cancelDownloadCommand = new RelayCommand(CancelDownload)); }
        }

        public bool NoDownloadedItems   
        {
            get { return DownloadedEpisodes.Count == 0; }
        }

        public bool NoDownloadingItems  
        {
            get { return Downloads.Count == 0; }
        }

        public RelayCommand<EpisodeDataModel> DownloadedClicked 
        {
            get { return _downloadedClicked ?? (_downloadedClicked = new RelayCommand<EpisodeDataModel>(DownloadedClick)); }
        }

        private void DownloadedClick(EpisodeDataModel episodeDataModel)
        {
            foreach (var downloadedEpisode in DownloadedEpisodes)
            {
                downloadedEpisode.IsSelected = false;
            }
            if (episodeDataModel == null)
            {
                SelectedDownloaded = null;
                RefreshPermissions();
                return;
            }
            episodeDataModel.IsSelected = true;
            SelectedDownloaded = episodeDataModel;
            RefreshPermissions();
        }

        public EpisodeDataModel SelectedDownloaded { get; set; }

        public bool CanDeleteDownloaded
        {
            get { return SelectedDownloaded != null; }
        }

        public RelayCommand DeleteDownloadedCommand
        {
            get { return _deleteDownloadedCommand ?? (_deleteDownloadedCommand = new RelayCommand(DeleteDownloaded)); }
        }

        private async void DeleteDownloaded()
        {
            if(SelectedDownloaded == null) return;
            await _downloadService.DeleteDownloadedEpisode(SelectedDownloaded.Model);
            UpdateLists();
        }

        private void CancelDownload()
        {
            foreach (var downloadEpisodeStatus in Downloads.Where(x => x.IsSelected))
            {
                _downloadService.CancelDownload(downloadEpisodeStatus.DownloadId);
                var task = activeDownloads.FirstOrDefault(x => x.Guid == downloadEpisodeStatus.DownloadId);
                if(task != null) activeDownloads.Remove(task);
                UpdateLists();
            }
        }

        private void ResumeDownload()
        {
            foreach (var downloadEpisodeStatus in Downloads.Where(x => x.IsSelected))
            {
                _downloadService.ResumeDownload(downloadEpisodeStatus.DownloadId);
            }
        }

        private void DownloadPause()
        {
            foreach (var downloadEpisodeStatus in Downloads.Where(x=>x.IsSelected))
            {
                _downloadService.PauseDownload(downloadEpisodeStatus.DownloadId);
            }
        }

        public void RefreshPermissions()
        {
            OnPropertyChanged("CanCancelDownload");
            OnPropertyChanged("CanPauseDownload");
            OnPropertyChanged("CanResumeDownload");
            OnPropertyChanged("CanDeleteDownloaded");
            OnPropertyChanged("IsAppbarOpen");
        }

        public void LoadDownloadList()
        {
            cts = new CancellationTokenSource();
            activeDownloads = new List<DownloadOperation>();
            UpdateLists();
        }

        private async Task HandleDownloadAsync(DownloadOperation download, bool start)
        {
            try
            {
                // Store the download so we can pause/resume.
                activeDownloads.Add(download);

                Progress<DownloadOperation> progressCallback = new Progress<DownloadOperation>(DownloadProgress);
                if (start)
                {
                    // Start the download and attach a progress handler.
                    await download.StartAsync().AsTask(cts.Token, progressCallback);
                }
                else
                {
                    // The download was already running when the application started, re-attach the progress handler.
                    await download.AttachAsync().AsTask(cts.Token, progressCallback);
                }
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Download cancelled.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error" + ex);
            }
            finally
            {
                activeDownloads.Remove(download);
                UpdateLists();
            }
        }

        private async void UpdateLists()
        {
            try
            {
                IReadOnlyList<DownloadOperation> downloads = await BackgroundDownloader.GetCurrentDownloadsAsync();
                Downloads.Clear();
                var downloadList = await _downloadService.GetDownloadingEpisodes();
                if (downloads.Count > 0)
                {
                    List<Task> tasks = new List<Task>();
                    foreach (DownloadOperation download in downloads)
                    {
                        var episodeDto = downloadList[download.Guid];
                        var episode = EpisodeDtoFactory.Create(episodeDto, null);
                        Downloads.Add(new DownloadEpisodeStatus(episode, download));
                        // Attach progress and completion handlers.
                        tasks.Add(HandleDownloadAsync(download, false));
                    }
                }
            }
            catch (Exception)
            {

            }
            OnPropertyChanged("NoDownloadingItems");

            var list = await _downloadService.GetDownloadedEpisodes();
            if (list == null) return;
            DownloadedEpisodes.Clear();
            var t = list.Count > 5 ? list.Count - 6 : 0;
            for (int i = list.Count; i > t; i--)
            {
                var downloadedEpisode = list[i-1];
                var epi = EpisodeDtoFactory.Create(downloadedEpisode, null);
                DownloadedEpisodes.Add(new EpisodeDataModel(epi));
            }
            OnPropertyChanged("NoDownloadedItems");
        }


        private void DownloadProgress(DownloadOperation download)
        {
            var downloadFull = Downloads.FirstOrDefault(x => x.DownloadId == download.Guid);
            if (downloadFull == null) return;
            try
            {
                downloadFull.Percentage = download.Progress.BytesReceived * 100 / download.Progress.TotalBytesToReceive;
                downloadFull.IsInternetDown = download.Progress.Status == BackgroundTransferStatus.PausedNoNetwork;
                downloadFull.IsPauseByUser = download.Progress.Status == BackgroundTransferStatus.PausedByApplication;

                RefreshPermissions();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public void OpenEpisode(DownloadEpisodeStatus episode)
        {
            if (Math.Abs(episode.Percentage - 100.00) > 0.02) return;
            App.RootFrame.Navigate(typeof(FastEpisodeViewer), episode.Episode.Title);
        }

        public void OpenEpisode(EpisodeDataModel episode)
        {
            App.RootFrame.Navigate(typeof(FastEpisodeViewer), episode.Title);    
        }

       
    }
}

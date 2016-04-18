using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Notifications;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.Data.Shows;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Services.PlatformServices;

namespace Shiftv.PlatformServices
{
    public class DownloadService : IDownloadService
    {
        private static readonly StorageFolder LocalFolder = ApplicationData.Current.LocalFolder;

        private static List<DownloadOperation> ActiveDownloads
        {
            get
            {
                return _active ?? (_active = new List<DownloadOperation>());
            }
        }
        private static CancellationTokenSource _cts;
        private static List<DownloadOperation> _active;

        public async void DoDownload(Uri uri, IEpisode episode)
        {
            _cts = new CancellationTokenSource();
            var downloader = new BackgroundDownloader();
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(episode.Title.Replace(" ", "") + ".mp4",
                CreationCollisionOption.ReplaceExisting);
            DownloadOperation download = downloader.CreateDownload(uri, file);
            AddDownloading(download.Guid, EpisodeDtoFactory.GetDto(episode));
            await DownloadHandler(download, episode, true);
        }

        private async Task DownloadHandler(DownloadOperation download, IEpisode episode, bool start)
        {
            try
            {
                // Store the download so we can pause/resume.
                ActiveDownloads.Add(download);
                if (start)
                {
                    SendToast(episode, "added to download queue!");
                    // Start the download and attach a progress handler.
                    await download.StartAsync().AsTask(_cts.Token);
                }
                else
                {
                    // The download was already running when the application started, re-attach the progress handler.
                    await download.AttachAsync().AsTask(_cts.Token);
                }
                SendToast(episode, "download completed!");
                AddCompletedDownload(episode);
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Download cancelled.");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                ActiveDownloads.Remove(download);
            }
        }

        private void SendToast(IEpisode episode, string text)
        {
            ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText04;
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(episode.Title.ToUpper()));
            toastTextElements[1].AppendChild(toastXml.CreateTextNode(""));
            toastTextElements[2].AppendChild(toastXml.CreateTextNode(text));
            XmlNodeList toastImageAttributes = toastXml.GetElementsByTagName("image");
            ((XmlElement)toastImageAttributes[0]).SetAttribute("src", episode.Images.Screenshot.Thumb);
            ((XmlElement)toastImageAttributes[0]).SetAttribute("alt", episode.Title);
            ToastNotification toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }


        private async void AddDownloading(Guid downId, EpisodeDto episode)
        {
            if (DownloadList == null) await GetDownloadingList();
            if (DownloadList != null && !DownloadList.ContainsValue(episode))
            {
                DownloadList.Add(downId, episode);
                StorageFile sampleFile = await LocalFolder.CreateFileAsync("episodesDownloading.txt",
                    CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(sampleFile, JsonConvert.SerializeObject(DownloadList));
            }
        }

        private async void AddCompletedDownload(IEpisode episode)
        {
            if (DownloadedEpisodes == null) await GetDownloadedList();
            var epi = EpisodeDtoFactory.GetDto(episode);
            if (DownloadedEpisodes != null && !DownloadedEpisodes.Contains(epi))
            {
                if (DownloadedEpisodes.Count >= 5)
                {
                    var firstEpisode = DownloadedEpisodes.FirstOrDefault();
                    if (firstEpisode == null) return;
                    var file = await ApplicationData.Current.LocalFolder.GetFileAsync(firstEpisode.Title.Replace(" ", "") + ".mp4");
                    await file.DeleteAsync();
                    var epi2 = DownloadedEpisodes.FirstOrDefault(x => x.Title == episode.Title && x.Ids.TvDbId == episode.Ids.TvDbId && x.Number == episode.Number && x.Season == episode.Season);
                    DownloadedEpisodes.Remove(epi2);
                }
                DownloadedEpisodes.Add(epi);
                await SaveDownloadedEpisodes();

            }
        }

        private async Task SaveDownloadedEpisodes()
        {
            StorageFile sampleFile = await LocalFolder.CreateFileAsync("episodesDownloaded.txt",
                CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(sampleFile, JsonConvert.SerializeObject(DownloadedEpisodes));
        }

        public async Task DeleteDownloadedEpisode(IEpisode episode)
        {
            try
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync(episode.Title.Replace(" ", "") + ".mp4");
                await file.DeleteAsync();
                var epi = DownloadedEpisodes.FirstOrDefault(x => x.Title == episode.Title && x.Ids.TvDbId == episode.Ids.TvDbId && x.Number == episode.Number && x.Season == episode.Season);
                DownloadedEpisodes.Remove(epi);
                await SaveDownloadedEpisodes();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public Dictionary<Guid, EpisodeDto> DownloadList { get; set; }

        public List<EpisodeDto> DownloadedEpisodes { get; set; }

        private async Task GetDownloadedList()
        {
            try
            {
                StorageFile sampleFile = await LocalFolder.GetFileAsync("episodesDownloaded.txt");
                String timestamp = await FileIO.ReadTextAsync(sampleFile);
                DownloadedEpisodes = JsonConvert.DeserializeObject<List<EpisodeDto>>(timestamp);
            }
            catch (Exception)
            {
                DownloadedEpisodes = new List<EpisodeDto>();
            }
        }

        private async Task GetDownloadingList()
        {
            try
            {
                StorageFile sampleFile = await LocalFolder.GetFileAsync("episodesDownloading.txt");
                String timestamp = await FileIO.ReadTextAsync(sampleFile);
                DownloadList = JsonConvert.DeserializeObject<Dictionary<Guid, EpisodeDto>>(timestamp);
            }
            catch (Exception)
            {
                DownloadList = new Dictionary<Guid, EpisodeDto>();
            }

        }

        public async Task<Dictionary<Guid, EpisodeDto>> GetDownloadingEpisodes()
        {
            if (DownloadList == null) await GetDownloadingList();
            return DownloadList;
        }

        public void PauseDownload(Guid downloadId)
        {
            var download = ActiveDownloads.FirstOrDefault(x => x.Guid == downloadId);
            if (download == null) return;
            download.Pause();
        }

        public void ResumeDownload(Guid downloadId)
        {
            var download = ActiveDownloads.FirstOrDefault(x => x.Guid == downloadId);
            if (download == null) return;
            download.Resume();
        }

        public void CancelDownload(Guid downloadId)
        {
            var download = ActiveDownloads.FirstOrDefault(x => x.Guid == downloadId);
            if (download == null) return;
            download.AttachAsync().Cancel();
            ActiveDownloads.Remove(download);
        }

        public async Task<List<EpisodeDto>> GetDownloadedEpisodes()
        {
            if (DownloadedEpisodes == null) await GetDownloadedList();
            return DownloadedEpisodes;
        }

        public async void ResumeDownloads()
        {
            IReadOnlyList<DownloadOperation> downloads;
            try
            {
                _cts = new CancellationTokenSource();
                downloads = await BackgroundDownloader.GetCurrentDownloadsAsync();
                if (downloads.Count <= 0)
                    return;
                await GetDownloadingList();
                foreach (DownloadOperation op in downloads)
                {
                    // op.Resume();
                    var episode = DownloadList[op.Guid];
                    if (episode != null) await DownloadHandler(op, EpisodeDtoFactory.Create(episode, null), false);
                }
            }
            catch (Exception ex)    
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using BugSense;
using Microsoft.ApplicationInsights;
//using Microsoft.ApplicationInsights.Telemetry.WindowsStore;
using Newtonsoft.Json;
using Shiftv.Common;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Services;
using Shiftv.Core.Models.Shows;
using Shiftv.DataModel;
using Shiftv.Global;
using Shiftv.Helpers;
using Shiftv.Views.Shows.Player;

namespace Shiftv.ViewModels.Shows.Pages
{
    public class DashboardViewModel : TvShowGridViewBase
    {
        private MultiObservableCollection<ShowDataModelProgress> _listWatchingShows;
        private ShowDataModelProgress _selectedShowProgress;
        private Thickness _margin;
        private ObservableCollection<ChartClassHelper> _globalChart;
        private int _totalAired;
        private int _totalToSee;
        private int _totalCompleted;
        private EpisodeDataModel _randomEpisode;
        private MiniShowDataModel _showDataModel;
        private bool _isLoadingData;
        private double _imageOpacity;
        private RelayCommand _randomReloadClicked;
        private List<IShowProgress> _model;
        private RelayCommand _episodeClicked;
        private bool _isClickProcess;
        private ObservableCollection<EpisodeDataModel> _episodesFromSelectedShow;
        private string _titleHubEpisodes;
        private bool _isLoadingEpisodes;
        private RelayCommand<EpisodeDataModel> _episodeFromListClicked;
        private MultiObservableCollection<EpisodeDataModel> _episodesFromSelectedShow1;
        private double _randomImageOpacity;

        public DashboardViewModel()
        {
            LoadData();
            //BugSenseHandler.Instance.SendEventAsync("TvShows/Dashboard");
            var tc = new TelemetryClient();
            tc.TrackPageView("TvShows/Dashboard");
        }

        public override sealed async void LoadData()
        {
            IsDataLoaded = false;
            var t = await CoreServices.Show.GetShowProgress();
            switch (t.Result)
            {
                case StandardResults.Ok:
                    if (t.Data.Count == 0)
                    {
                        ErrorGettingData = true;
                        IsDataLoaded = true;
                    }
                    else
                    {
                        Process(t);
                        App.UpdateLiveTileWithData(t);
                    }

                    break;
                case StandardResults.Offline:
                    if (NumberRequested == 0)
                    {
                        ErrorGettingData = true;
                        IsDataLoaded = true;
                    }
                    break;
                case StandardResults.Error:
                    if (NumberRequested == 0)
                    {
                        ErrorGettingData = true;
                        IsDataLoaded = true;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            OnPropertyChanged("IsToShowData");
        }

        private void Process(DataResult<List<IShowProgress>> t)
        {
            if (t != null && t.IsOk && t.Data != null && t.Data.Count > 0)
            {
                _model = t.Data;
                var list = new List<ShowDataModelProgress>();
                foreach (IShowProgress u in t.Data)
                {
                    if (u.Show != null && u.EpisodesLeft.Count > 0) list.Add(new ShowDataModelProgress(u));
                }
                ListWatchingShows = new MultiObservableCollection<ShowDataModelProgress>(list.Take(10), list);
              
                var sum = t.Data.Sum(x => x.TotalEpisodes);
                if (sum != null)
                    TotalCompleted = sum.Value - t.Data.Sum(x => x.EpisodesLeft.Count);
                TotalToSee = t.Data.Sum(x => x.EpisodesLeft.Count);
                TotalAired = TotalToSee + TotalCompleted;
                SetRandomEpisode();
            }
            if (DesignMode.DesignModeEnabled)
            {
                SelectedShowProgress = ListWatchingShows.First();
            }
            IsDataLoaded = true;
            OnPropertyChanged("IsToShowData");
        }

        private async void SetRandomEpisode()
        {
            IsLoadingData = true;
            if (_model == null)
            {
                IsLoadingData = false;
                return;
            }
            var showprogress = _model.Where(x => x.EpisodesLeft.Count > 0).PickRandom();
            var episode = showprogress.EpisodesLeft.OrderBy(x => x.FirstAired).FirstOrDefault();
            if (episode != null)
            {
                RandomShow = new MiniShowDataModel(showprogress.Show);
                RandomEpisode = new EpisodeDataModel(episode, true, showprogress.Show.Fanart);
                RandomImageOpacity = 1.0;
            }
            else
            {
                Debug.WriteLine("randomSeason is null");
            }
            IsLoadingData = false;
        }

        public ShowDataModelProgress SelectedShowProgress
        {
            get { return _selectedShowProgress; }
            set
            {
                SetProperty(ref _selectedShowProgress, value);
                if (value == null) return;
                ProcessSelected();
                ProcessEpisodes();
            }
        }

        private void ProcessSelected()
        {
            foreach (var showProgress in ListWatchingShows)
            {
                showProgress.Show.IsSelected = false;
            }
            if (SelectedShowProgress != null) SelectedShowProgress.Show.IsSelected = true;
        }

        private async void ProcessEpisodes()
        {
            IsLoadingEpisodes = true;
            TitleHubEpisodes = "episodes left to see";
            var list = SelectedShowProgress.EpisodesLeft.Where(x => x.Season != 0).OrderBy(x => x.FirstAired).ToList();
            EpisodesFromSelectedShow = new MultiObservableCollection<EpisodeDataModel>(list.Take(10), list);
            await Task.Delay(500);
            IsLoadingEpisodes = false;
            OnPropertyChanged("IsShowSelected");
        }

        public MultiObservableCollection<ShowDataModelProgress> ListWatchingShows
        {
            get { return _listWatchingShows; }
            set { SetProperty(ref _listWatchingShows, value); }
        }

        public Thickness MarginTopHeight
        {
            get { return _margin; }
            set
            {

                SetProperty(ref _margin, value);
            }
        }

        public ObservableCollection<ChartClassHelper> Errors
        {
            get
            {
                return _globalChart ?? (_globalChart = new ObservableCollection<ChartClassHelper>());
            }
        }

        public int TotalAired
        {
            get { return _totalAired; }
            set { SetProperty(ref _totalAired, value); }
        }

        public int TotalToSee
        {
            get { return _totalToSee; }
            set { SetProperty(ref _totalToSee, value); }
        }

        public int TotalCompleted
        {
            get { return _totalCompleted; }
            set { SetProperty(ref _totalCompleted, value); }
        }

        public EpisodeDataModel RandomEpisode
        {
            get { return _randomEpisode; }
            set { SetProperty(ref _randomEpisode, value); }
        }

        public MiniShowDataModel RandomShow
        {
            get { return _showDataModel; }
            set { SetProperty(ref _showDataModel, value); }
        }

        public bool IsLoadingData
        {
            get { return _isLoadingData; }
            set { SetProperty(ref _isLoadingData, value); }
        }

        public double ImageOpacity
        {
            get { return _imageOpacity; }
            set { SetProperty(ref _imageOpacity, value); }
        }

        public double RandomImageOpacity
        {
            get { return _randomImageOpacity; }
            set { SetProperty(ref _randomImageOpacity, value); }
        }

        public RelayCommand RandomReloadClicked
        {
            get { return _randomReloadClicked ?? (_randomReloadClicked = new RelayCommand(SetRandomEpisode)); }
        }

        public RelayCommand EpisodeClicked
        {
            get { return _episodeClicked ?? (_episodeClicked = new RelayCommand(EpisodeClick)); }
        }

        public bool IsClickProcess
        {
            get { return _isClickProcess; }
            set { SetProperty(ref _isClickProcess, value); }
        }

        public MultiObservableCollection<EpisodeDataModel> EpisodesFromSelectedShow
        {
            get { return _episodesFromSelectedShow1; }
            set { SetProperty(ref _episodesFromSelectedShow1, value); }
        }

        public string TitleHubEpisodes
        {
            get { return _titleHubEpisodes; }
            set { SetProperty(ref _titleHubEpisodes, value); }
        }

        public bool IsLoadingEpisodes
        {
            get { return _isLoadingEpisodes; }
            set { SetProperty(ref _isLoadingEpisodes, value); }
        }

        public RelayCommand<EpisodeDataModel> EpisodeFromListClicked
        {
            get
            {
                return _episodeFromListClicked ??
                       (_episodeFromListClicked = new RelayCommand<EpisodeDataModel>(EpisodeFromListClick));
            }
        }

        public bool IsToShowData
        {
            get
            {
                if (!IsDataLoaded) return false;
                if (ErrorGettingData) return false;
                return true;
            }
        }

        public bool IsShowSelected
        {
            get { return SelectedShowProgress != null; }
        }

        public async void EpisodeFromListClick(EpisodeDataModel obj)
        {
            if (obj == null) return;
            obj.IsLoadToView = true;
            obj.ImageOpacity = 0.5;
            await EpisodeClickFinalAction(obj, SelectedShowProgress.Show);
            obj.IsLoadToView = false;
            obj.ImageOpacity = 0.1;
        }

        private async void EpisodeClick()
        {
            ImageOpacity = 0.5;
            IsClickProcess = true;
            await EpisodeClickFinalAction(RandomEpisode, RandomShow);
            IsClickProcess = false;
        }

        private async Task EpisodeClickFinalAction(EpisodeDataModel episode, MiniShowDataModel selectedShow)
        {
            await CoreServices.Show.SetCurrent(selectedShow.Model);
            var data = new EpisodeViewerDataModelMini(episode.Season, episode.Number);
            App.RootFrame.Navigate(typeof(EpisodeViewer), JsonConvert.SerializeObject(data));
        }

        public async void SeenTapped(EpisodeDataModel episodeDataModel)
        {
            episodeDataModel.IsProcessing = true;
            var selectedShow = SelectedShowProgress.Model.Show;
            if (selectedShow == null)
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorSetAsSeenText_Capital"), ShiftvHelpers.GetTranslation("ErrorSetAsSeenTitle_Capital"));
                messageDialog.ShowAsync();
                episodeDataModel.IsProcessing = false;
                return;
            }
            var res = await CoreServices.Episode.SetAsSeen(episodeDataModel.Model, selectedShow);
            if (res.IsOk && res.Data.Success)
            {
                EpisodesFromSelectedShow.Remove(episodeDataModel);
                SelectedShowProgress.EpisodesToSee--;
                RemoveFromList(new List<EpisodeDataModel> { episodeDataModel });
            }
            else
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorSetAsSeenText_Capital"), ShiftvHelpers.GetTranslation("ErrorSetAsSeenTitle_Capital"));
                messageDialog.ShowAsync();
            }
            //  IsRatingOrWatchlisting = false;
            episodeDataModel.IsProcessing = false;
        }

        private void RemoveFromList(List<EpisodeDataModel> episodeDataModels)
        {
            foreach (var episodeDataModel in episodeDataModels)
            {
                var epi = SelectedShowProgress.EpisodesLeft.FirstOrDefault(x => x.Number == episodeDataModel.Number);
                if (epi == null) continue;
                SelectedShowProgress.EpisodesLeft.Remove(epi);
            }
        }

        public async void SeenTopTapped(EpisodeDataModel episodeDataModel)
        {
            OnPropertyChanged("CanUseAppBarButtons");
            var selectedShow = SelectedShowProgress.Model.Show;
            var index = EpisodesFromSelectedShow.IndexOf(episodeDataModel);
            var list = new List<IEpisode>();
            var listToClean = new List<EpisodeDataModel>();
            for (int i = 0; i <= index; i++)
            {
                list.Add(EpisodesFromSelectedShow[i].Model);
                listToClean.Add(EpisodesFromSelectedShow[i]);
                EpisodesFromSelectedShow[i].IsProcessing = true;
            }
            if (selectedShow == null)
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorSetAsSeenText_Capital"), ShiftvHelpers.GetTranslation("ErrorSetAsSeenTitle_Capital"));
                messageDialog.ShowAsync();
                SetAllEpisodesProcessingToFalse();

                return;
            }

            var res = await CoreServices.Episode.SetAsSeen(list, selectedShow);
            if (res.IsOk && res.Data != null)
            {
                foreach (var dataModel in listToClean)
                {
                    EpisodesFromSelectedShow.Remove(dataModel);
                }
                EpisodesFromSelectedShow.Remove(episodeDataModel);
                RemoveFromList(listToClean);
                SelectedShowProgress.EpisodesToSee = SelectedShowProgress.EpisodesToSee - listToClean.Count;
            }
            else
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorSetAsSeenText_Capital"), ShiftvHelpers.GetTranslation("ErrorSetAsSeenTitle_Capital"));
                messageDialog.ShowAsync();
            }

            //  IsRatingOrWatchlisting = false;
            OnPropertyChanged("CanUseAppBarButtons");
            SetAllEpisodesProcessingToFalse();
        }

        private void SetAllEpisodesProcessingToFalse()
        {
            foreach (var episodeDataModel in EpisodesFromSelectedShow.Where(x => x.IsProcessing))
            {
                episodeDataModel.IsProcessing = false;
            }
        }
    }

    public class ChartClassHelper
    {
        public string Label { get; set; }
        public int Value { get; set; }
    }

    public class ShowDataModelProgress : ViewModelBase
    {
        private IShowProgress _model;
        private int _episodesSeen;
        private int _episodesToSee;
        private MiniShowDataModel _show;
        private List<EpisodeDataModel> _episodesLeft;

        public ShowDataModelProgress(IShowProgress prog)
        {
            _model = prog;
            Show = new MiniShowDataModel(prog.Show);
            EpisodesToSee = prog.EpisodesLeft.Count;
            if (prog.TotalEpisodes != null) EpisodesSeen = prog.TotalEpisodes.Value - prog.EpisodesLeft.Count;
            EpisodesLeft = new List<EpisodeDataModel>();
            foreach (var episode in prog.EpisodesLeft)
            {
                EpisodesLeft.Add(new EpisodeDataModel(episode, true, prog.Show.Fanart));
            }
        }


        public MiniShowDataModel Show
        {
            get { return _show; }
            set { SetProperty(ref _show, value); }
        }

        public List<EpisodeDataModel> EpisodesLeft
        {
            get { return _episodesLeft; }
            set { SetProperty(ref _episodesLeft, value); }
        }

        public int EpisodesToSee
        {
            get { return _episodesToSee; }
            set { SetProperty(ref _episodesToSee, value); }
        }

        public int EpisodesSeen
        {
            get { return _episodesSeen; }
            set { SetProperty(ref _episodesSeen, value); }
        }

        public string FormattedStats
        {
            get { return string.Format("{0} AIRED, {1} LEFT", 0, _model.EpisodesLeft.Count); }
        }

        public IShowProgress Model { get { return _model; } }
    }
}

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Newtonsoft.Json;
using Shiftv.Common;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Services;
using Shiftv.DataModel;
using Shiftv.Global;
using Shiftv.Helpers;
using Shiftv.Views.Shows.Episodes;
using Shiftv.Views.Shows.Player;

namespace Shiftv.ViewModels.Shows.Episodes
{
    public class ListEpisodesViewModel : ViewModelBase
    {
        private ObservableCollection<EpisodeDataModel> _episodes;
        private ShowDataModel _show;
        private Thickness _margin;
        private int _season;
        private SeasonDataModel _seasonModel;
        private ObservableCollection<EpisodeDataModel> _selectedEpisodes;
        private RelayCommand _setAllAsSeen;
        private RelayCommand _setAllAsUnseen;
        private RelayCommand _setLove;
        private RelayCommand _setHate;
        private RelayCommand _addAllToWatchList;
        private RelayCommand _removeAllFromWatchList;
        private bool _isLoadingEpisodes;
        private bool _isRatingOrWatchlisting;
        private RelayCommand _retryClicked;
        private RelayCommand _setAllAsSeenAndPrevious;
        private bool _isRatingVisible;
        private RelayCommand _openRating;
        private bool _isAppBarOpen;
        private bool _isOnlyOneSelected;

        public ListEpisodesViewModel()
        {
            if (DesignMode.DesignModeEnabled) LoadData(3);
        }

        public async void LoadData(int season)
        {
            IsLoadingEpisodes = true;
            ErrorGettingData = false;
            _season = season;
            var serie = CoreServices.Show.GetCurrentShow();
            if (serie == null) return;
            Show = new ShowDataModel(serie);
            var seasonData = serie.Seasons.FirstOrDefault(a => a.Number == season);
            Season = new SeasonDataModel(seasonData);
            foreach (var episode in seasonData.Episodes)
            {
                Episodes.Add(new EpisodeDataModel(episode, true, Show.Image.Fanart));
            }
            IsLoadingEpisodes = false;
            //var x = await CoreServices.Episode.GetEpisodesBySeason(_season);
            //switch (x.Result)
            //{
            //    case StandardResults.Ok:

            //        break;
            //    case StandardResults.Offline:
            //        IsLoadingEpisodes = false;
            //        ErrorGettingData = true;
            //        break;
            //    case StandardResults.Error:
            //        IsLoadingEpisodes = false;
            //        ErrorGettingData = true;
            //        break;
            //    default:
            //        throw new ArgumentOutOfRangeException();
            //}
        }



        public ObservableCollection<EpisodeDataModel> Episodes
        {
            get
            {
                return _episodes ?? (_episodes = new ObservableCollection<EpisodeDataModel>());
            }
        }

        public SeasonDataModel Season
        {
            get { return _seasonModel; }
            set
            {
                SetProperty(ref _seasonModel, value);
            }
        }
        public ShowDataModel Show
        {
            get { return _show; }
            set
            {
                SetProperty(ref _show, value);
            }
        }

        public Thickness MarginTopHeight
        {
            get { return _margin; }
            set
            {

                SetProperty(ref _margin, value);
            }
        }

        public string SeasonTitle
        {
            get { return string.Format("{2} {0} - {1} {3}", _season, Season.Episodes.Count, ShiftvHelpers.GetTranslation("Season_Upper"), ShiftvHelpers.GetTranslation("Episodes_Upper")).ToUpper(); }
        }

        public ObservableCollection<EpisodeDataModel> SelectedEpisodes
        {
            get
            {
                return _selectedEpisodes ?? (_selectedEpisodes = new ObservableCollection<EpisodeDataModel>());
            }
        }

        public bool IsAllUnseen
        {
            get
            {
                if (SelectedEpisodes.Count == 0) return false;
                var a = SelectedEpisodes.All(x => !x.Model.Watched);
                return a;
            }
        }
        public bool IsAllSeen
        {
            get
            {
                if (SelectedEpisodes.Count == 0) return false;
                var a = SelectedEpisodes.All(x => x.Model.Watched);
                return a; 
            }
        }

        public RelayCommand SetAllAsSeen
        {
            get { return _setAllAsSeen ?? (_setAllAsSeen = new RelayCommand(SetAllEpisodesAsSeen)); }
        }

        public RelayCommand SetAllAsUnseen
        {
            get { return _setAllAsUnseen ?? (_setAllAsUnseen = new RelayCommand(SetAllEpisodesAsUnseen)); }
        }


        public bool CanLove
        {
            get { return false; }
        }

        public bool CanHate
        {
            get { return false; }
        }

        public bool CanAddWatchlist
        {
            get { return false; }
        }

        public bool CanRemoveWatchlist
        {
            get { return false; }
        }

        public RelayCommand AddAllToWatchList
        {
            get { return _addAllToWatchList ?? (_addAllToWatchList = new RelayCommand(AddAllEpisodesToWatchList)); }
        }

        public RelayCommand RemoveAllFromWatchList
        {
            get { return _removeAllFromWatchList ?? (_removeAllFromWatchList = new RelayCommand(RemoveAllEpisodesFromWatchList)); }
        }

        public bool IsLoadingEpisodes
        {
            get { return _isLoadingEpisodes; }
            set { SetProperty(ref _isLoadingEpisodes, value); }
        }




        private async void RemoveAllEpisodesFromWatchList()
        {
            IsRatingOrWatchlisting = true;
            OnPropertyChanged("CanUseAppBarButtons");
            var res = await CoreServices.Episode.RemoveAllFromWatchlist(SelectedEpisodes.Select(a => a.Model).ToList());
            if (res.IsOk && res.Data.Status == RequestResults.Success)
            {
                UpdatePermissions();
            }
            else
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorRemoveFromWatchListText_Capital"), ShiftvHelpers.GetTranslation("ErrorRemoveFromWatchListTitle_Capital"));
                messageDialog.ShowAsync();
            }
            IsRatingOrWatchlisting = false;
            OnPropertyChanged("CanUseAppBarButtons");
        }

        private async void AddAllEpisodesToWatchList()
        {
            IsRatingOrWatchlisting = true;
            OnPropertyChanged("CanUseAppBarButtons");
            var res = await CoreServices.Episode.AddEpisodesToWatchlist(SelectedEpisodes.Select(a => a.Model).ToList());
            if (res.IsOk && res.Data.Status == RequestResults.Success)
            {
                UpdatePermissions();
            }
            else
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorAddToWatchListText_Capital"), ShiftvHelpers.GetTranslation("ErrorAddToWatchListTitle_Capital"));
                messageDialog.ShowAsync();
            }
            IsRatingOrWatchlisting = false;
            OnPropertyChanged("CanUseAppBarButtons");
        }


        private async void SetAllEpisodesAsUnseen()
        {
            IsRatingOrWatchlisting = true;
            OnPropertyChanged("CanUseAppBarButtons");
            var res = await CoreServices.Episode.SetAsUnseen(SelectedEpisodes.Select(a => a.Model).ToList());
            if (res.IsOk && res.Data !=null)
            {
                foreach (var episode in res.Data.Where(x => x.Success))
                {
                    var episodeUpd = Episodes.FirstOrDefault(x => x.Model.Ids.TraktId == episode.Episode.Ids.TraktId);
                    episodeUpd.RefreshData();
                }
                UpdatePermissions();

            }
            else
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorSetAsUnseenText_Capital"), ShiftvHelpers.GetTranslation("ErrorSetAsUnseenTitle_Capital"));
                messageDialog.ShowAsync();
            }
            IsRatingOrWatchlisting = false;
            OnPropertyChanged("CanUseAppBarButtons");
        }


        private async void SetAllEpisodesAsSeen()
        {
            IsRatingOrWatchlisting = true;
            OnPropertyChanged("CanUseAppBarButtons");
            var res = await CoreServices.Episode.SetAsSeen(SelectedEpisodes.Select(a => a.Model).ToList());
            if (res.IsOk)
            {
                UpdatePermissions();
                foreach (var episode in res.Data.Where(x => x.Success))
                {
                    var episodeUpd = Episodes.FirstOrDefault(x => x.Model.Ids.TraktId == episode.Episode.Ids.TraktId);
                    episodeUpd.RefreshData();
                }
            }
            else
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorSetAsSeenText_Capital"), ShiftvHelpers.GetTranslation("ErrorSetAsSeenTitle_Capital"));
                messageDialog.ShowAsync();
            }
            IsRatingOrWatchlisting = false;
            OnPropertyChanged("CanUseAppBarButtons");
        }

        public void OpenEpisode(EpisodeDataModel episode)
        {
            var data = new EpisodeViewerDataModelMini(Season.Number, episode.Number);
            var x = JsonConvert.SerializeObject(data);
            App.RootFrame.Navigate(typeof(EpisodeViewer), x);
        }

        public bool CanUseAppBarButtons
        {
            get
            {
                if (!IsUserLogged) return false;
                if (IsRatingOrWatchlisting) return false;
                return true;
            }
        }


        public bool IsRatingOrWatchlisting
        {
            get { return _isRatingOrWatchlisting; }
            set { SetProperty(ref _isRatingOrWatchlisting, value); }
        }

        public RelayCommand SetAllAsSeenAndPrevious
        {
            get
            {
                return _setAllAsSeenAndPrevious ??
                       (_setAllAsSeenAndPrevious = new RelayCommand(SetAllAsSeenAndPreviousAction));
            }
        }

        private async void SetAllAsSeenAndPreviousAction()
        {
            IsRatingOrWatchlisting = true;
            OnPropertyChanged("CanUseAppBarButtons");
            var selected = SelectedEpisodes.Select(a => a.Model).ToList();
            if (selected.Count > 0)
            {
                var first = selected.OrderBy(x => x.Number).FirstOrDefault();
                if (first != null) selected.AddRange(Episodes.Where(x => x.Number < first.Number).Select(x => x.Model).ToList());
            }
            foreach (SeasonDataModel seasonDataModel in Show.Seasons.Where(x => x.Number < Season.Number))
            {
                selected.AddRange(seasonDataModel.Episodes.Where(x=>!x.Watched));
            }
            var res = await CoreServices.Episode.SetAsSeen(selected);
            if (res.IsOk)
            {
                foreach (var episode in res.Data.Where(x=>x.Success))
                {
                    var episodeUpd = Episodes.FirstOrDefault(x => x.Model.Ids.TraktId == episode.Episode.Ids.TraktId);
                    if(episodeUpd!=null)episodeUpd.RefreshData();
                }
                UpdatePermissions();
            }
            else
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorSetAsSeenText_Capital"), ShiftvHelpers.GetTranslation("ErrorSetAsSeenTitle_Capital"));
                messageDialog.ShowAsync();
            }
            IsRatingOrWatchlisting = false;
            OnPropertyChanged("CanUseAppBarButtons");
        }

        public async void UpdatePermissions()
        {
            //var updatedEpisode = await CoreServices.Episode.GetEpisodesBySeason(Season.Number);
            //if (updatedEpisode != null && updatedEpisode.IsOk && updatedEpisode.Data != null)
            //{
            //    foreach (var episode in Episodes)
            //    {
            //        var updatedFullEpi = updatedEpisode.Data.FirstOrDefault(x => x.Number == episode.Number);
            //        if (updatedFullEpi != null)
            //        {
            //            episode.RefreshData(updatedFullEpi);
            //        }
            //    }
            //}
            OnPropertyChanged("CanUseAppBarButtons");
            OnPropertyChanged("IsAllUnseen");
            OnPropertyChanged("IsAllSeen");
            OnPropertyChanged("CanAddWatchlist");
            OnPropertyChanged("CanRemoveWatchlist");
            OnPropertyChanged("CanHate");
            OnPropertyChanged("CanLove");
            OnPropertyChanged("IsAppBarOpen");
            OnPropertyChanged("IsOnlyOneSelected");
        }

        public RelayCommand OpenRating
        {
            get { return _openRating ?? (_openRating = new RelayCommand(OpenRatingPopUp)); }
        }

        public bool IsRatingVisible
        {
            get { return _isRatingVisible; }
            set { SetProperty(ref _isRatingVisible, value); }
        }

        public int RatingValue { get; set; }

        //public RelayCommand RatingValueChanged
        //{
        //    get { return _ratingValueChanged ?? (_ratingValueChanged = new RelayCommand(RatingValueChangedSubmit)); }
        //}

        public async Task RatingValueChangedSubmit(double newValue)
        {
            var res = await CoreServices.Episode.RateEpisodes((int)newValue, SelectedEpisodes.Select(a => a.Model).ToList());
            if (!res.IsOk)
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorRatingText_Capital"), ShiftvHelpers.GetTranslation("ErrorRatingTitle_Capital"));
                messageDialog.ShowAsync();
            }
            else
            {
                foreach (var episodeDataModel in SelectedEpisodes)
                {
                    episodeDataModel.RefreshData();
                }
            }
            await Task.Delay(400);
            IsRatingVisible = false;
        }

        private void OpenRatingPopUp()
        {
            if (SelectedEpisodes.Count == 1)
            {
                RatingValue = SelectedEpisodes[0].RatedValue != null ? SelectedEpisodes[0].RatedValue.Value : 0;
            }
            else
            {
                RatingValue = 0;
            }
            IsAppBarOpen = false;
            IsRatingVisible = true;
        }

        public bool IsAppBarOpen
        {
            get { return _isAppBarOpen; }
            set { SetProperty(ref _isAppBarOpen, value); }
        }

        public bool IsOnlyOneSelected
        {
            get { return SelectedEpisodes.Count == 1; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using BugSense;
using Microsoft.ApplicationInsights;
//using Microsoft.ApplicationInsights.Telemetry.WindowsStore;
using Newtonsoft.Json;
using Shiftv.Common;
using Shiftv.Contracts.Data.JsonTrakt;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Services;
using Shiftv.DataModel;
using Shiftv.Helpers;
using Shiftv.ViewModels.Shows.Pages;
using Shiftv.Views.Network;
using Shiftv.Views.Shows;
using Shiftv.Views.Shows.Episodes;
using Shiftv.Views.Shows.Player;

namespace Shiftv.ViewModels.Shows
{
    public class SerieViewModel : TvShowGridViewBase
    {
        private ShowDataModel _show;
        private Thickness _margin;
        private MultiObservableCollection<CommentsDataModel> _comments;
        private RelayCommand _addComment;
        private string _newComment;
        private bool _isReview;
        private bool _isSpoiler;
        private ObservableCollection<UserDataModel> _watchingNow;
        private RelayCommand _setLove;
        private RelayCommand _setHate;
        private RelayCommand _addToWatchList;
        private RelayCommand _removeFromWatchList;
        private RelayCommand<SeasonDataModel> _seasonClicked;
        private RelayCommand<CommentsDataModel> _commentClicked;
        private ScrollableObservableCollection _episodes;
        private RelayCommand<EpisodeDataModel> _episodeClicked;
        private bool _isOtherEpisodesVisible;
        private RelayCommand _nextEpisodeTapped;
        private RelayCommand _otherEpisodesTapped;
        private double _nextEpisodeOpacity;
        private double _otherEpisodesOpacity;
        private bool _isLoadingEpisodes;
        private MultiObservableCollection<CastDataModel> _actors;
        private MultiObservableCollection<SeasonDataModel> _seasons;
        private bool _isRatingOrWatchlisting;
        private bool _isLoadingComments;
        private bool _isToNotify;
        private bool _isTryingToComment;
        private RelayCommand _openRating;
        private bool _isRatingVisible;
        private int _ratingValue;
        private RelayCommand _ratingValueChanged;
        private RelayCommand _addToHistory;
        private bool _isAddHistoryVisible;
        private RelayCommand<SeasonDataModel> _addHistorySeasonChoosed;
        private ObservableCollection<EpisodeDataModel> _historyEpisodes;
        private RelayCommand<EpisodeDataModel> _addHistoryEpisodeChoosed;
        private bool _isHistoryEpisodeSelected;
        private RelayCommand _addToHistoryFinish;
        private bool _isAddingToHistory;
        private bool _isHistoryFirstStep;
        private bool _isHistorySecondStep;
        private bool _isHistoryThirdStep;
        private MultiObservableCollection<SeasonDataModel> _historySeasons;
        private RelayCommand _forceShowRefresh;
        private RelayCommand<string> _commentTextChanged;
        private string _numLetters;
        private SolidColorBrush _numLettersColorBrush;
        private RelayCommand _cancelAddToHistory;
        private bool _isForcingUpdate;

        public SerieViewModel()
        {
            LoadData();
        }

        public override sealed async void LoadData()
        {
            try
            {
                var serie = CoreServices.Show.GetCurrentShow();
                if (serie == null) return;
                Insights(serie);
                Show = new ShowDataModel(serie);
                if (Show.RatedValue != null) RatingValue = Show.RatedValue.Value;
                OnPropertyChanged("NextEpisode");
                OnPropertyChanged("IsToNotify");
                NextEpisodeOpacity = 1;
                OtherEpisodesOpacity = 0.5;
                NumLetters = "0";
                LoadActors();
                LoadSeasons();
                LoadComments();
                var user = CoreServices.User.GetCurrentUser();
                if (user != null)
                {
                    CurrentUserAccount = new UserDataModel(user.UserSettings.User);
                }
            }
            catch (Exception)
            {

                var a = 0;
            }

        }

        private void Insights(IShow serie)
        {
            if (serie == null) return;
            //var properties = new Windows.Foundation.Collections.PropertySet();
            //properties["ShowName"] = serie.Title;
            //properties["ImdbId"] = serie.ImdbId;
            //ClientAnalyticsChannel.Default.LogEvent("TvShows/Detail", properties);
            //BugSenseHandler.Instance.SendEventAsync("TvShows/Detail");
            var tc = new TelemetryClient();
            tc.TrackPageView("TvShows/Detail");
        }

        private void LoadSeasons()
        {
            if (Show.Seasons == null) return;
            var listSeasons = Show.Seasons.Where(x=>x.Episodes != null && x.Episodes.Count > 0).OrderByDescending(x => x.Number).ToList();
            if (listSeasons != null && listSeasons.Count > 0) Seasons = new MultiObservableCollection<SeasonDataModel>(listSeasons.Take(10), listSeasons);

            var listForHistory = Show.Seasons.Where(x=>x.Episodes != null && x.Episodes.Count > 0 && x.Episodes.Any(a=>a.FirstAiredDate < DateTime.Now)).Where(x=>x.Number != 0).OrderByDescending(x => x.Number).ToList();
            if (listForHistory != null && listForHistory.Count > 0) HistorySeasons = new MultiObservableCollection<SeasonDataModel>(listForHistory.Take(10), listForHistory);
        }

        public MultiObservableCollection<SeasonDataModel> HistorySeasons
        {
            get { return _historySeasons; }
            set { SetProperty(ref _historySeasons, value); }
        }

        private async void LoadActors()
        {
            var people = await CoreServices.Show.GetPeople();
            if (people.IsOk)
            {
                var parsed = people.Data.Cast.Select(x => new CastDataModel(x)).ToList();
                Actors = new MultiObservableCollection<CastDataModel>(parsed.Take(10), parsed);
            }
        }

        private async void LoadComments()
        {
            IsLoadingComments = true;
            var list = new List<CommentsDataModel>();
            var comments = await CoreServices.Comments.GetCommentsShowById();
            if (comments.IsOk && comments.Data != null && comments.Data.Count > 0)
            {
                list.AddRange(comments.Data.Select(comment => new CommentsDataModel(comment)));
            }
            if (list.Count > 0)
            {
                Comments = new MultiObservableCollection<CommentsDataModel>(list.OrderByDescending(x => x.CommentDateTime).Take(10), list.OrderByDescending(x => x.CommentDateTime));
            }
            IsLoadingComments = false;
        }

        public ScrollableObservableCollection Episodes
        {
            get { return _episodes; }
            set { SetProperty(ref _episodes, value); }
        }
        public MultiObservableCollection<CommentsDataModel> Comments
        {
            get { return _comments; }
            set { SetProperty(ref _comments, value); }
        }

        public MultiObservableCollection<CastDataModel> Actors
        {
            get { return _actors; }
            set { SetProperty(ref _actors, value); }
        }

        public MultiObservableCollection<SeasonDataModel> Seasons
        {
            get { return _seasons; }
            set { SetProperty(ref _seasons, value); }
        }

        private async Task LoadEpisodes()
        {
            IsLoadingEpisodes = true;
            var list = new List<EpisodeDataModel>();
            var lastseasonNumber = 1;
            var showLastSeason = Show.Seasons.Where(x=>x.Episodes != null && x.Episodes.Count > 0).OrderByDescending(x => x.Number).FirstOrDefault();
            if (showLastSeason != null) lastseasonNumber = showLastSeason.Number;
            while (list.Count == 0 && lastseasonNumber != 0)
            {
                if (showLastSeason != null)
                {
                    var epi = showLastSeason.Episodes;

                    list.AddRange(epi.Where(x => x.FirstAiredDate != null && x.FirstAiredDate < DateTime.Now).OrderByDescending(x => x.Number).Select(episode => new EpisodeDataModel(episode, true, Show.Image.Fanart)));
                }
                lastseasonNumber--;
            }
            Episodes = list.Count > 0 ? new ScrollableObservableCollection(list) : null;
            IsLoadingEpisodes = false;
        }


        public ShowDataModel Show
        {
            get { return _show; }
            set
            {
                SetProperty(ref _show, value);
            }
        }

        public EpisodeDataModel NextEpisode
        {
            get
            {
                return _show == null ? null : _show.ToModel().GetNextEpisode() != null ? new EpisodeDataModel(_show.ToModel().GetNextEpisode(), true, Show.Image.Fanart) : new EpisodeDataModel();
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

        public string NextEpisodeTitle
        {
            get
            {
                if (NextEpisode == null) return null;
                return NextEpisode.Model.FirstAiredDate >= DateTime.Now ? ShiftvHelpers.GetTranslation("NextEpisode") : ShiftvHelpers.GetTranslation("LastEpisode");
            }
        }


        public string NewComment
        {
            get { return _newComment; }
            set
            {
                SetProperty(ref _newComment, value);
                CommentTextChangedAction();
            }
        }

        public bool IsSpoiler
        {
            get { return _isSpoiler; }
            set { SetProperty(ref _isSpoiler, value); }
        }

        public bool IsReview
        {
            get { return _isReview; }
            set { SetProperty(ref _isReview, value); }
        }
        public bool NoCommentsAvailable
        {
            get { return !IsLoadingComments && (Comments == null || Comments.Count == 0); }
        }

        public RelayCommand AddComment
        {
            get { return _addComment ?? (_addComment = new RelayCommand(AddCommentToShow)); }
        }

        public ObservableCollection<UserDataModel> WatchingNow
        {
            get { return _watchingNow ?? (_watchingNow = new ObservableCollection<UserDataModel>()); }
        }

        public RelayCommand SetLove
        {
            get { return _setLove ?? (_setLove = new RelayCommand(SetShowLove)); }
        }

        public RelayCommand SetHate
        {
            get { return _setHate ?? (_setHate = new RelayCommand(SetShowHate)); }
        }

        public RelayCommand AddToWatchList
        {
            get { return _addToWatchList ?? (_addToWatchList = new RelayCommand(AddShowToWatchList)); }
        }

        public RelayCommand RemoveFromWatchList
        {
            get { return _removeFromWatchList ?? (_removeFromWatchList = new RelayCommand(RemoveShowFromWatchList)); }
        }

        public bool IsInWatchList
        {
            get
            {
                return Show != null && Show.InWatchlist;
            }
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

        public RelayCommand<SeasonDataModel> SeasonClicked
        {
            get { return _seasonClicked ?? (_seasonClicked = new RelayCommand<SeasonDataModel>(SeasonClick)); }
        }

        public RelayCommand<CommentsDataModel> CommentClicked
        {
            get { return _commentClicked ?? (_commentClicked = new RelayCommand<CommentsDataModel>(CommentClick)); }
        }

        public RelayCommand<EpisodeDataModel> EpisodeClicked
        {
            get { return _episodeClicked ?? (_episodeClicked = new RelayCommand<EpisodeDataModel>(EpisodeClick)); }
        }

        public bool IsOtherEpisodesVisible
        {
            get { return _isOtherEpisodesVisible; }
            set
            {
                SetProperty(ref _isOtherEpisodesVisible, value);
                if (value)
                {
                    OtherEpisodesOpacity = 1;
                    NextEpisodeOpacity = 0.5;
                }
                else
                {
                    OtherEpisodesOpacity = 0.5;
                    NextEpisodeOpacity = 1;
                }
            }
        }

        public RelayCommand NextEpisodeTapped
        {
            get { return _nextEpisodeTapped ?? (_nextEpisodeTapped = new RelayCommand(NextEpisodeTap)); }
        }

        private void NextEpisodeTap()
        {
            IsOtherEpisodesVisible = false;
        }

        public RelayCommand OtherEpisodesTapped
        {
            get { return _otherEpisodesTapped ?? (_otherEpisodesTapped = new RelayCommand(OtherEpisodesTap)); }
        }

        public double NextEpisodeOpacity
        {
            get { return _nextEpisodeOpacity; }
            set { SetProperty(ref _nextEpisodeOpacity, value); }
        }

        public double OtherEpisodesOpacity
        {
            get { return _otherEpisodesOpacity; }
            set { SetProperty(ref _otherEpisodesOpacity, value); }
        }

        public bool IsLoadingEpisodes
        {
            get { return _isLoadingEpisodes; }
            set { SetProperty(ref _isLoadingEpisodes, value); }
        }

        private async void OtherEpisodesTap()
        {
            IsOtherEpisodesVisible = true;
            if (Episodes == null) await LoadEpisodes();
        }

        private void EpisodeClick(EpisodeDataModel obj)
        {
            var data = new EpisodeViewerDataModelMini(obj.Season, obj.Number);
            var x = JsonConvert.SerializeObject(data);
            App.RootFrame.Navigate(typeof(EpisodeViewer), x);
        }

        public void CommentClick(CommentsDataModel obj)
        {
            if (obj == null) return;
            App.RootFrame.Navigate(typeof(UserProfileView), obj.User.Username);
        }

        private void SeasonClick(SeasonDataModel obj)
        {
            if (obj == null) return;
            App.RootFrame.Navigate(typeof(EpisodesList), obj.Number);
        }

        private async void RemoveShowFromWatchList()
        {
            IsRatingOrWatchlisting = true;
            OnPropertyChanged("CanUseAppBarButtons");
            var x = await CoreServices.Show.RemoveFromWatchlist();
            if (x.IsOk && x.Data.Status == RequestResults.Success)
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

        private async void AddShowToWatchList()
        {
            IsRatingOrWatchlisting = true;
            OnPropertyChanged("CanUseAppBarButtons");
            var x = await CoreServices.Show.AddToWatchlist();
            if (x.IsOk && x.Data.Status == RequestResults.Success)
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

        private async void SetShowHate()
        {
            IsRatingOrWatchlisting = true;
            OnPropertyChanged("CanUseAppBarButtons");
            var x = await CoreServices.Show.RateShow(0);
            if (x.IsOk && x.Data.Status)
            {
                UpdatePermissions();
            }
            else
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorRatingText_Capital"), ShiftvHelpers.GetTranslation("ErrorRatingTitle_Capital"));
                messageDialog.ShowAsync();
            }
            IsRatingOrWatchlisting = false;
            OnPropertyChanged("CanUseAppBarButtons");
        }

        private async void SetShowLove()
        {
            IsRatingOrWatchlisting = true;
            OnPropertyChanged("CanUseAppBarButtons");
            var x = await CoreServices.Show.RateShow(10);
            if (x.IsOk && x.Data.Status)
            {
                UpdatePermissions();
            }
            else
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorRatingText_Capital"), ShiftvHelpers.GetTranslation("ErrorRatingTitle_Capital"));
                messageDialog.ShowAsync();
            }
            IsRatingOrWatchlisting = false;
            OnPropertyChanged("CanUseAppBarButtons");
        }

        public bool IsRatingOrWatchlisting
        {
            get { return _isRatingOrWatchlisting; }
            set { SetProperty(ref _isRatingOrWatchlisting, value); }
        }

        public double ImagePercentage
        {
            get { return 0.65; }
        }

        public double TitlePercentage
        {
            get { return 0.42; }
        }

        public bool IsLoadingComments
        {
            get { return _isLoadingComments; }
            set
            {
                SetProperty(ref _isLoadingComments, value);
                OnPropertyChanged("NoCommentsAvailable");
            }
        }


        public bool IsTryingToComment
        {
            get { return _isTryingToComment; }
            set
            {
                SetProperty(ref _isTryingToComment, value);
                OnPropertyChanged("IsCommentButtonEnabled");
            }
        }

        public bool IsCommentButtonEnabled
        {
            get
            {
                if (IsTryingToComment) return false;
                var intNumLetters = Convert.ToInt32(NumLetters);
                if (intNumLetters <= 4) return false;
                return true;
            }
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

        public RelayCommand AddToHistory
        {
            get { return _addToHistory ?? (_addToHistory = new RelayCommand(ShowAddToHistory)); }
        }

        public bool IsAddHistoryVisible
        {
            get { return _isAddHistoryVisible; }
            set { SetProperty(ref _isAddHistoryVisible, value); }
        }

        public RelayCommand<SeasonDataModel> AddHistorySeasonChoosed
        {
            get
            {
                return _addHistorySeasonChoosed ??
                       (_addHistorySeasonChoosed = new RelayCommand<SeasonDataModel>(AddHistorySeasonChoosedEvent));
            }
        }

        private void AddHistorySeasonChoosedEvent(SeasonDataModel obj)
        {
            if(obj == null) return;
            foreach (var season in HistorySeasons)
            {
                season.IsSelected = false;
            }
            obj.IsSelected = true;
            HistoryEpisodes.Clear();
            foreach (var episodes in obj.Episodes.Where(x=>x.FirstAiredDate != null && x.FirstAiredDate < DateTime.Now))
            {
                HistoryEpisodes.Add(new EpisodeDataModel(episodes));
            }
            OnPropertyChanged("IsHistoryEpisodeSelected");
            OnPropertyChanged("IsSeasonHistorySelected");
        }

        public ObservableCollection<EpisodeDataModel> HistoryEpisodes
        {
            get { return _historyEpisodes ?? (_historyEpisodes = new ObservableCollection<EpisodeDataModel>()); }
        }

        public RelayCommand<EpisodeDataModel> AddEpisodeHistoryChoosed
        {
            get
            {
                return _addHistoryEpisodeChoosed ??
                       (_addHistoryEpisodeChoosed = new RelayCommand<EpisodeDataModel>(AddEpisodeHistoryChoosedAction));
            }
        }

        public bool IsHistoryEpisodeSelected
        {
            get
            {
                return HistoryEpisodes != null && HistoryEpisodes.Any(x=>x.IsSelected);
            }
        }

        public bool IsSeasonHistorySelected
        {
            get { return HistorySeasons != null && HistorySeasons.Any(x => x.IsSelected); }
        }

        public RelayCommand AddToHistoryFinish
        {
            get { return _addToHistoryFinish ?? (_addToHistoryFinish = new RelayCommand(AddToHistoryFinishAction)); }
        }

        public bool IsAddingToHistory
        {
            get { return _isAddingToHistory; }
            set { SetProperty(ref _isAddingToHistory, value); }
        }

        public bool IsHistoryFirstStep
        {
            get { return _isHistoryFirstStep; }
            set { SetProperty(ref _isHistoryFirstStep, value); }
        }

        private async void AddToHistoryFinishAction()
        {
            IsHistoryFirstStep = false;
            IsHistorySecondStep = true;

            var totalListEpisodes = new List<IEpisode>();
            var selectedSeason = HistorySeasons.FirstOrDefault(x => x.IsSelected);
            var selectedEpisode = HistoryEpisodes.FirstOrDefault(x => x.IsSelected);
            if(selectedSeason == null||selectedEpisode == null) return;
            foreach (var season in HistorySeasons.Where(x=>x.Number <= selectedSeason.Number))
            {
                if (season.Number == selectedSeason.Number)
                {
                    totalListEpisodes.AddRange(season.Episodes.Where(x=>x.Number <= selectedEpisode.Number));
                }
                else
                {
                    totalListEpisodes.AddRange(season.Episodes);
                }
            }
            var res = await CoreServices.Episode.SetAsSeen(totalListEpisodes);
            IsHistorySecondStep = false;
            if (res.IsOk)
            {
                IsHistoryThirdStep = true;
                LoadSeasons();
            }
            else
            {
                IsAddHistoryVisible = false;
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorSetAsSeenText_Capital"), ShiftvHelpers.GetTranslation("ErrorSetAsSeenTitle_Capital"));
                messageDialog.ShowAsync();

            }
        }

        public bool IsHistorySecondStep
        {
            get { return _isHistorySecondStep; }
            set { SetProperty(ref _isHistorySecondStep, value); }
        }

        public bool IsHistoryThirdStep
        {
            get { return _isHistoryThirdStep; }
            set { SetProperty(ref _isHistoryThirdStep, value); }
        }

        public RelayCommand ForceShowRefresh
        {
            get { return _forceShowRefresh ?? (_forceShowRefresh = new RelayCommand(ForceShowRefreshAction)); }
        }

        public bool CanHadToHistory
        {
            get
            {
                if (Seasons == null) return false;
                if (Seasons.Any(x => x.Episodes.Any(a => a.Watched)))
                {
                    return false;
                }
                return true;
            }
        }


        public string NumLetters
        {
            get { return _numLetters; }
            set { SetProperty(ref _numLetters, value); }
        }

        public SolidColorBrush NumLettersColorBrush
        {
            get { return _numLettersColorBrush; }
            set { SetProperty(ref _numLettersColorBrush, value); }
        }

        public RelayCommand  CancelAddToHistory
        {
            get { return _cancelAddToHistory ?? (_cancelAddToHistory = new RelayCommand(CancelAddToHistoryAction)); }
        }

        public bool IsForcingUpdate
        {
            get { return _isForcingUpdate; }
            set { SetProperty(ref _isForcingUpdate, value); }
        }

        private void CancelAddToHistoryAction()
        {
            IsHistoryFirstStep = false;
        }

        private void CommentTextChangedAction()
        {
            var countWords = NewComment.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Count();
            NumLetters = countWords.ToString();
            if (countWords <= 4)
            {
                NumLettersColorBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                NumLettersColorBrush = new SolidColorBrush(Colors.White);
            }
            OnPropertyChanged("IsCommentButtonEnabled");
        }

        private async void ForceShowRefreshAction()
        {
            IsForcingUpdate = true;
            var updateShowRes = await CoreServices.Show.ForceUpdate();
            if (updateShowRes.IsOk)
            {
                App.RootFrame.Navigate(typeof(SeriePage));
            }
            else
            {
                IsForcingUpdate = false;
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorUpdatingShowText_Capital"), ShiftvHelpers.GetTranslation("ErrorUpdatingShowTitle_Capital"));
                messageDialog.ShowAsync();
            }
        }

        private void AddEpisodeHistoryChoosedAction(EpisodeDataModel obj)
        {
            if(obj == null) return;
            foreach (var episodeDataModel in HistoryEpisodes)
            {
                episodeDataModel.IsSelected = false;
            }
            obj.IsSelected = true;
            OnPropertyChanged("IsHistoryEpisodeSelected");
        }

        private void ShowAddToHistory()
        {
            IsAddHistoryVisible = true;
            IsHistoryFirstStep = true;
            foreach (var seasonDataModel in HistorySeasons)
            {
                seasonDataModel.IsSelected = false;
            }
            HistoryEpisodes.Clear();
        }

        //public RelayCommand RatingValueChanged
        //{
        //    get { return _ratingValueChanged ?? (_ratingValueChanged = new RelayCommand(RatingValueChangedSubmit)); }
        //}

        public async Task RatingValueChangedSubmit(double newValue)
        {
            var res = await CoreServices.Show.RateShow((int)newValue);
            if (!res.IsOk)
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorRatingText_Capital"), ShiftvHelpers.GetTranslation("ErrorRatingTitle_Capital"));
                messageDialog.ShowAsync();
            }
            else
            {
                Show.RefreshData();
            }
            await Task.Delay(400);
            IsRatingVisible = false;
        }

        private void OpenRatingPopUp()
        {
            IsAppBarOpen = false;
            IsRatingVisible = true;
        }


        private void UpdatePermissions()
        {
            var updatedShow = CoreServices.Show.GetCurrentShow();
            if (updatedShow != null)
            {
                Show.RefreshData(updatedShow);
            }
            OnPropertyChanged("CanLove");
            OnPropertyChanged("CanHate");
            OnPropertyChanged("IsInWatchList");
            OnPropertyChanged("CanUseAppBarButtons");
        }

        private async void AddCommentToShow()
        {
            if (string.IsNullOrEmpty(NewComment)) return;
            IsTryingToComment = true;
            var resComment = await CoreServices.Comments.CommentsShow(NewComment, IsSpoiler, IsReview);
            if (!resComment.IsOk)
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorAddingCommentText_Capital"), ShiftvHelpers.GetTranslation("ErrorAddingCommentTitle_Capital"));
                messageDialog.ShowAsync();
            }
            else
            {
                IsSpoiler = false;
                IsReview = false;
                NewComment = "";
                LoadComments();
            }
            IsTryingToComment = false;
        }
    }

    public class ScrollableObservableCollection : ObservableCollection<EpisodeDataModel>, ISupportIncrementalLoading
    {
        public ScrollableObservableCollection(IEnumerable<EpisodeDataModel> episodes)
            : base(episodes)
        {
            HasMoreItems = true;
        }
        public bool HasMoreItems { get; set; }
        private bool isRunning = false;
        public Windows.Foundation.IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            if (isRunning) //not thread safe
            {
                return null;
            }
            isRunning = true;

            return AsyncInfo.Run(async c =>
            {
                var lastSeason = this.Last().Season;

                var episodes = new ObservableCollection<EpisodeDataModel>();
                try
                {
                    var currentShow = CoreServices.Show.GetCurrentShow();
                    var season = currentShow.Seasons.FirstOrDefault(x => x.Number == lastSeason - 1);
                    foreach (var episode in season.Episodes.Where(x => x.FirstAiredDate != null && x.FirstAiredDate < DateTime.Now).OrderByDescending(x => x.Number))
                    {
                        episodes.Add(new EpisodeDataModel(episode, true, currentShow.Images.Fanart));
                    }

                }
                catch
                {
                    HasMoreItems = false;
                }

                foreach (var s in episodes)
                {
                    if (!this.Any(x => x.Season == s.Season && x.Number == s.Number))
                    {
                        this.Add(s);
                    }
                    else
                    {
                        Debug.WriteLine("avoiding duplicate");
                    }
                }

                HasMoreItems = episodes.Any();
                isRunning = false;
                return new LoadMoreItemsResult()
                {
                    Count = (uint)episodes.Count
                };

            });
        }
    }
}

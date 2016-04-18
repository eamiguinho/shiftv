using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using BugSense;
//using Microsoft.ApplicationInsights.Telemetry.WindowsStore;
using Shiftv.Common;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Services;
using Shiftv.Core.Models.Shows;
using Shiftv.DataModel;
using Shiftv.Helpers;
using Shiftv.Views.Movies.Player;
using Shiftv.Views.Shows.Episodes;

namespace Shiftv.ViewModels.Movies
{
    public class MovieViewModel : ViewModelBase
    {
        private MovieDataModel _movie;
        private Thickness _margin;
        private UserDataModel _userAccount;
        private RelayCommand _addComment;
        private string _newComment;
        private bool _isReview;
        private bool _isSpoiler;
        private ObservableCollection<UserDataModel> _watchingNow;
        private RelayCommand _setLove;
        private RelayCommand _setHate;
        private RelayCommand _addToWatchList;
        private RelayCommand _removeFromWatchList;
        private Uri _trailerUri;
        private RelayCommand _playMovieClicked;
        private bool _isPlayerFullScreen;
        private MultiObservableCollection<CommentsDataModel> _comments;
        private MultiObservableCollection<CastDataModel> _actors;
        private bool _isRatingOrWatchlisting;
        private bool _watched;
        private RelayCommand _setAsSeen;
        private RelayCommand _setAsUnseen;
        private bool _isLoadingComments;
        private bool _isTryingToComment;
        private bool _isLoadingActors;
        private RelayCommand _openRating;
        private bool _isRatingVisible;
        private bool _isAppBarOpen;
        private SolidColorBrush _numLettersColorBrush;
        private string _numLetters;

        public MovieViewModel()
        {
            LoadData();
        }

        private async void LoadData()
        {
            var movie = CoreServices.Movie.GetCurrentMovie();
            if (movie == null) return;
            Insights(movie);
            Movie = new MovieDataModel(movie);
            OnPropertyChanged("Trailer");
            OnPropertyChanged("IsTitle2Filled");
            if (Movie.RatedValue != null) RatingValue = Movie.RatedValue.Value;
            LoadActors();
            var user = CoreServices.User.GetCurrentUser();
            if (user != null)
            {
                CurrentUserAccount = new UserDataModel(user.UserSettings.User);
            }
            var heightVar = 440;
            if(Movie.Title.Count() > 17) heightVar = 530;
            if (Movie.Title.Count() > 35) heightVar = 610;
            var bounds = Window.Current.Bounds;
            MarginTopHeight = new Thickness(0, bounds.Height - heightVar, 0, 0);
            LoadComments();
        }

        private void Insights(IMovie movie)
        {
            if (movie == null) return;
            //var properties = new Windows.Foundation.Collections.PropertySet();
            //properties["MovieName"] = movie.Title;
            //properties["ImdbId"] = movie.ImdbId;
          //  ClientAnalyticsChannel.Default.LogEvent("Movie/Detail", properties);
            BugSenseHandler.Instance.SendEventAsync("Movie/Detail");
        }

        private async void LoadActors()
        {
            IsLoadingActors = true;
            var people = await CoreServices.Movie.GetPeople();
            if (people.IsOk)
            {
                var parsed = people.Data.Cast.Select(x => new CastDataModel(x)).ToList();
                Actors = new MultiObservableCollection<CastDataModel>(parsed.Take(10), parsed);
            } 
            IsLoadingActors = false;

        }

        public bool IsLoadingActors
        {
            get { return _isLoadingActors; }
            set { SetProperty(ref _isLoadingActors, value); }
        }

        public MultiObservableCollection<CastDataModel> Actors
        {
            get { return _actors; }
            set { SetProperty(ref _actors, value); }
        }


        private async void LoadComments()
        {
            IsLoadingComments = true;
            var list = new List<CommentsDataModel>();
            var comments = await CoreServices.Comments.GetCommentsMovie();
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

        public MovieDataModel Movie
        {
            get { return _movie; }
            set
            {
                SetProperty(ref _movie, value);
            }
        }

        public Uri Trailer
        {
            get
            {
                if (_trailerUri != null)
                {
                    return _trailerUri;
                }
                GetTrailerUrl();
                return null;
            }
            set
            {
                _trailerUri = value;
                SetProperty(ref _trailerUri, value);
            }
        }

        private async void GetTrailerUrl()
        {
            Trailer = await _movie.GetTrailerUrl();
        }

        public Thickness MarginTopHeight
        {
            get { return _margin; }
            set
            {

                SetProperty(ref _margin, value);
            }
        }


        public UserDataModel CurrentUserAccount
        {
            get { return _userAccount; }
            set
            {
                SetProperty(ref _userAccount, value);
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

        public MultiObservableCollection<CommentsDataModel> Comments
        {
            get { return _comments; }
            set { SetProperty(ref _comments,value); }
        }

        public bool NoCommentsAvailable
        {
            get { return !IsLoadingComments && (Comments == null || Comments.Count == 0); }
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



        public RelayCommand AddComment
        {
            get { return _addComment ?? (_addComment = new RelayCommand(AddCommentToShow)); }
        }

        public ObservableCollection<UserDataModel> WatchingNow
        {
            get { return _watchingNow ?? (_watchingNow = new ObservableCollection<UserDataModel>()); }
        }

        public bool CanLove
        {
            get
            {
                if (!Movie.IsLoveOrHate) return true;
                return !Movie.IsLoved;
            }
        }

        public bool CanHate
        {
            get
            {
                if (!Movie.IsLoveOrHate) return true;
                return Movie.IsLoved;
            }
        }


        public RelayCommand AddToWatchList
        {
            get { return _addToWatchList ?? (_addToWatchList = new RelayCommand(AddShowToWatchList)); }
        }

        public RelayCommand RemoveFromWatchList
        {
            get { return _removeFromWatchList ?? (_removeFromWatchList = new RelayCommand(RemoveShowFromWatchList)); }
        }

        public RelayCommand SetAsSeen
        {
            get { return _setAsSeen ?? (_setAsSeen = new RelayCommand(SetMovieAsSeen)); }
        }
        public RelayCommand SetAsUnseen
        {
            get { return _setAsUnseen ?? (_setAsUnseen = new RelayCommand(SetMovieAsUnseen)); }
        }

        public bool IsInWatchList
        {
            get { return Movie.InWatchlist; }
        }

        public RelayCommand PlayMovieClicked
        {
            get { return _playMovieClicked ?? (_playMovieClicked = new RelayCommand(PlayMovieClick)); }
        }

        public bool IsPlayerFullScreen
        {
            get { return _isPlayerFullScreen; }
            set { SetProperty(ref _isPlayerFullScreen, value); }
        }

        private async void PlayMovieClick()
        {
            if ((DateTime.Now - DateTime.Parse(Movie.ToModel().Released)).TotalDays < 31)
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("MovieReleaseMessage"), ShiftvHelpers.GetTranslation("MovieReleaseTitle"));
                messageDialog.Commands.Add(new UICommand(
        ShiftvHelpers.GetTranslation("Yes"),
        new UICommandInvokedHandler(this.OpenMovie)));
                messageDialog.Commands.Add(new UICommand(
                    ShiftvHelpers.GetTranslation("No"), this.IgnoreMovie));
                 await messageDialog.ShowAsync();
            }
            else
            {
                App.RootFrame.Navigate(typeof(MoviePlayer));
            }
           
        }

        private void IgnoreMovie(IUICommand command)
        {
            
        }

        private void OpenMovie(IUICommand command)
        {
            App.RootFrame.Navigate(typeof(MoviePlayer));
        }

        private async void RemoveShowFromWatchList()
        {
            IsRatingOrWatchlisting = true;
            UpdatePermissions();
            var x = await CoreServices.Movie.RemoveFromWatchlist();
            if (x.IsOk && x.Data.Status == RequestResults.Success)
            {
                UpdatePermissions();
                Movie.RefreshData();
            }
            else
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorRemoveFromWatchListText_Capital"), ShiftvHelpers.GetTranslation("ErrorAddToWatchListTitle_Capital"));
                messageDialog.ShowAsync();
            }
            IsRatingOrWatchlisting = false;
            OnPropertyChanged("CanUseAppBarButtons");
        }

        public bool Watched
        {
            get { return Movie != null && Movie.Watched; }
        }

        private async void AddShowToWatchList()
        {
            IsRatingOrWatchlisting = true;
            UpdatePermissions();
            var x = await CoreServices.Movie.AddToWatchlist();
            if (x.IsOk && x.Data.Status == RequestResults.Success)
            {
                UpdatePermissions();
                Movie.RefreshData();
            }
            else
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorAddToWatchListText_Capital"), ShiftvHelpers.GetTranslation("ErrorAddToWatchListTitle_Capital"));
                messageDialog.ShowAsync();
            }
            IsRatingOrWatchlisting = false;
            OnPropertyChanged("CanUseAppBarButtons");
        }

        private async void SetMovieAsSeen()
        {
            IsRatingOrWatchlisting = true;
            UpdatePermissions();
            var res = await CoreServices.Movie.SetAsSeen();
            if (res.IsOk && res.Data.Status == RequestResults.Success)
            {
                UpdatePermissions();
                Movie.RefreshData();
            }
            else
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorSetAsSeenText_Capital"), ShiftvHelpers.GetTranslation("ErrorSetAsSeenTitle_Capital"));
                messageDialog.ShowAsync();
            }
            IsRatingOrWatchlisting = false;
            OnPropertyChanged("CanUseAppBarButtons");
        }
         private async void SetMovieAsUnseen()
        {
            IsRatingOrWatchlisting = true;
            UpdatePermissions();
            var res = await CoreServices.Movie.SetAsUnseen();
            if (res.IsOk && res.Data.Status == RequestResults.Success)
            {
                UpdatePermissions();
                Movie.RefreshData();
            }
            else
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorSetAsUnseenText_Capital"), ShiftvHelpers.GetTranslation("ErrorSetAsSeenTitle_Capital"));
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


    
        public bool CanUseAppBarButtons
        {
            get
            {
                if (!IsUserLogged) return false;
                if (IsRatingOrWatchlisting) return false;
                return true;
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
            var res = await CoreServices.Movie.RateMovie((int)newValue);
            if (!res.IsOk)
            {
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorRatingText_Capital"), ShiftvHelpers.GetTranslation("ErrorRatingTitle_Capital"));
                messageDialog.ShowAsync();
            }
            else
            {
                UpdatePermissions();
            }
            await Task.Delay(400);
            IsRatingVisible = false;
        }

        private void OpenRatingPopUp()
        {
            IsAppBarOpen = false;
            IsRatingVisible = true;
        }

        public bool IsAppBarOpen
        {
            get { return _isAppBarOpen; }
            set { SetProperty(ref _isAppBarOpen, value); }
        }


        private void UpdatePermissions()
        {
            var updatedMovie = CoreServices.Movie.GetCurrentMovie();
            if (updatedMovie != null)
            {
                Movie.RefreshData(updatedMovie);
            }
            OnPropertyChanged("IsRated");
            OnPropertyChanged("RatedValue");
            OnPropertyChanged("CanLove");
            OnPropertyChanged("CanHate");
            OnPropertyChanged("IsInWatchList");
            OnPropertyChanged("Watched");
            OnPropertyChanged("CanUseAppBarButtons");
        }

        private async void AddCommentToShow()
        {
            if (string.IsNullOrEmpty(NewComment)) return;
            IsTryingToComment = true;
            var res = await CoreServices.Comments.CommentsMovie(NewComment, IsSpoiler, IsReview);
            if (!res.IsOk)
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

        public void OpenSeason(SeasonDataModel season)
        {
            App.RootFrame.Navigate(typeof(EpisodesList), season.Number);
        }

        public Task<Uri> GetTrailer()
        {
            return _movie.GetTrailerUrl();
        }
    }
}

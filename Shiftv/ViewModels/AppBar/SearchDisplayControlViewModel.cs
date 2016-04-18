using System;
using System.Collections.ObjectModel;
using Windows.ApplicationModel;
using Windows.UI.Popups;
using Newtonsoft.Json;
using Shiftv.Common;
using Shiftv.Contracts.Domain.Activity;
using Shiftv.Contracts.Services;
using Shiftv.DataModel;
using Shiftv.Helpers;
using Shiftv.Views.Movies;
using Shiftv.Views.Network;
using Shiftv.Views.Shows;
using Shiftv.Views.Shows.Player;

namespace Shiftv.ViewModels.AppBar
{
    public class SearchDisplayControlViewModel : ViewModelBase
    {
        private RelayCommand<string> _querySubmittedClicked;
        private UserDataModel _currentUserAccount;
        private ObservableCollection<ActivityDataModel> _activities;
        private RelayCommand<ActivityDataModel> _activityClicked;
        private ObservableCollection<UserDataModel> _networkRequests;
        private bool _isLoadingData;
        private RelayCommand _userTapped;
        private ObservableCollection<MiniShowDataModel> _showSearchResult;
        private RelayCommand _tvShowsTapped;
        private bool _isTvShowsVisible;
        private bool _isMoviesVisible;
        private int _showSearchResultAmout;
        private ObservableCollection<MiniMovieDataModel> _movieSearchResult;
        private int _movieSearchResultAmount;
        private bool _isToShowResults;
        private RelayCommand _peopleTapped;
        private RelayCommand _moviesTapped;
        private ObservableCollection<UserDataModel> _userSearchResult;
        private int _userSearchResultAmout;
        private bool _isUserVisible;
        private RelayCommand<MiniShowDataModel> _showsClicked;
        private RelayCommand<MiniMovieDataModel> _moviesClicked;
        private RelayCommand<UserDataModel> _usersClicked;
        private bool _isLoadingUsers;
        private bool _isLoadingMovies;
        private bool _isLoadingShows;

        public SearchDisplayControlViewModel()
        {
            if (DesignMode.DesignModeEnabled) ProcessQuerySubmitted("falling");
        }

        public RelayCommand<string> QuerySubmittedClicked
        {
            get
            {
                return _querySubmittedClicked ?? (_querySubmittedClicked = new RelayCommand<string>(
          queryText =>
          {
              if (string.IsNullOrEmpty(queryText)) return;
              if (queryText.Length == 1) return;
              ProcessQuerySubmitted(queryText);
          }));
            }
        }

        private void ProcessQuerySubmitted(string queryText)
        {
            ClearData();
            IsToShowResults = true;
            IsTvShowsVisible = true;
            LoadShows(queryText);
            LoadMovies(queryText);
            //LoadUsers(queryText);
            SetVisibility();
            UpdateOpacities();
        }

        private async void LoadMovies(string queryText)
        {
            IsLoadingMovies = true;
            var movies = await CoreServices.Movie.SearchMoviesByKey(queryText);
            if (movies.IsOk)
            {
                foreach (var movie in movies.Data)
                {
                    MovieSearchResult.Add(new MiniMovieDataModel(movie));
                }
                MovieSearchResultAmout = MovieSearchResult.Count;
            }
            IsLoadingMovies = false;
        }

        //private async void LoadUsers(string queryText)
        //{
        //    IsLoadingUsers = true;
        //    var users = await CoreServices.User.SearchUserByKey(queryText);
        //    if (users.IsOk)
        //    {
        //        foreach (var user in users.Data)
        //        {
        //            UserSearchResult.Add(new UserProfileDataModel(user));
        //        }
        //        UserSearchResultAmout = UserSearchResult.Count;
        //    }
        //    IsLoadingUsers = false;
        //}

        private async void LoadShows(string queryText)
        {
            IsLoadingShows = true;
            var shows = await CoreServices.Show.SearchShowsByKey(queryText);
            if (shows.IsOk)
            {
                foreach (var show in shows.Data)
                {
                    ShowSearchResult.Add(new MiniShowDataModel(show));
                }
                ShowSearchResultAmout = ShowSearchResult.Count;
            }
            IsLoadingShows = false;
        }

        private void SetVisibility()
        {
            if (ShowSearchResultAmout > 0) IsTvShowsVisible = true;
            else if (MovieSearchResultAmout > 0) IsMoviesVisible = true;
            else if (UserSearchResultAmout > 0) IsUserVisible = true;
        }

        private void ClearData()
        {
            UserSearchResult.Clear();
            UserSearchResultAmout = 0;
            ShowSearchResult.Clear();
            ShowSearchResultAmout = 0;
            MovieSearchResult.Clear();
            MovieSearchResultAmout = 0;
            IsUserVisible = false;
            IsTvShowsVisible = false;
            IsMoviesVisible = false;
            IsToShowResults = false;
            OnPropertyChanged("IsLoadingData");
        }

        private void UpdateOpacities()
        {
            OnPropertyChanged("OpacityMovies");
            OnPropertyChanged("OpacityShows");
            OnPropertyChanged("OpacityUsers");
        }

        public int UserSearchResultAmout
        {
            get { return _userSearchResultAmout; }
            set
            {
                SetProperty(ref _userSearchResultAmout, value);
                OnPropertyChanged("UsersAmountFormatted");
            }
        }

        public bool IsToShowResults
        {
            get { return _isToShowResults; }
            set
            {
                SetProperty(ref _isToShowResults, value);

            }
        }

        public int MovieSearchResultAmout
        {
            get { return _movieSearchResultAmount; }
            set
            {
                SetProperty(ref _movieSearchResultAmount, value);
                OnPropertyChanged("MoviesAmountFormatted");
            }
        }

        public int ShowSearchResultAmout
        {
            get { return _showSearchResultAmout; }
            set
            {
                SetProperty(ref _showSearchResultAmout, value);
                OnPropertyChanged("TvShowsAmountFormatted");
            }
        }

        public UserDataModel CurrentUserAccount
        {
            get { return _currentUserAccount; }
            set { SetProperty(ref _currentUserAccount, value); }
        }

        public ObservableCollection<ActivityDataModel> Activities
        {
            get { return _activities ?? (_activities = new ObservableCollection<ActivityDataModel>()); }
        }

        public RelayCommand<ActivityDataModel> ActivityClicked
        {
            get { return _activityClicked ?? (_activityClicked = new RelayCommand<ActivityDataModel>(ActivityClick)); }
        }

        public ObservableCollection<UserDataModel> NetworkRequests
        {
            get { return _networkRequests ?? (_networkRequests = new ObservableCollection<UserDataModel>()); }
        }

        public bool NoFriendActivities
        {
            get { return _activities != null && _activities.Count == 0 && IsLoadingData == false; }
        }

        public bool IsLoadingData
        {
            get { return IsLoadingShows || IsLoadingMovies || IsLoadingUsers; }
        }

        public bool IsLoadingUsers
        {
            get { return _isLoadingUsers; }
            set
            {
                SetProperty(ref _isLoadingUsers, value);
                OnPropertyChanged("IsLoadingData");
            }
        }

        public bool IsLoadingMovies
        {
            get { return _isLoadingMovies; }
            set
            {
                SetProperty(ref _isLoadingMovies, value);
                OnPropertyChanged("IsLoadingData");
            }
        }

        public bool IsLoadingShows
        {
            get { return _isLoadingShows; }
            set
            {
                SetProperty(ref _isLoadingShows, value);
                OnPropertyChanged("IsLoadingData");
            }
        }

        public RelayCommand UserTapped
        {
            get { return _userTapped ?? (_userTapped = new RelayCommand(UserTap)); }
        }

        public ObservableCollection<MiniShowDataModel> ShowSearchResult
        {
            get { return _showSearchResult ?? (_showSearchResult = new ObservableCollection<MiniShowDataModel>()); }
        }

        public ObservableCollection<MiniMovieDataModel> MovieSearchResult
        {
            get { return _movieSearchResult ?? (_movieSearchResult = new ObservableCollection<MiniMovieDataModel>()); }
        }
        public ObservableCollection<UserDataModel> UserSearchResult
        {
            get { return _userSearchResult ?? (_userSearchResult = new ObservableCollection<UserDataModel>()); }
        }

        public RelayCommand TvShowsTapped
        {
            get { return _tvShowsTapped ?? (_tvShowsTapped = new RelayCommand(TvShowsTap)); }
        }

        private void TvShowsTap()
        {
            IsTvShowsVisible = true;
            IsMoviesVisible = false;
            IsUserVisible = false;
            UpdateOpacities();
        }
        private void PeopleTap()
        {
            IsTvShowsVisible = false;
            IsMoviesVisible = false;
            IsUserVisible = true;
            UpdateOpacities();
        }
        private void MoviesTap()
        {
            IsTvShowsVisible = false;
            IsMoviesVisible = true;
            IsUserVisible = false;
            UpdateOpacities();
        }


        public bool IsUserVisible
        {
            get { return _isUserVisible; }
            set { SetProperty(ref _isUserVisible, value); }
        }

        public bool IsMoviesVisible
        {
            get { return _isMoviesVisible; }
            set { SetProperty(ref _isMoviesVisible, value); }
        }

        public bool IsTvShowsVisible
        {
            get { return _isTvShowsVisible; }
            set { SetProperty(ref _isTvShowsVisible, value); }
        }

        public RelayCommand PeopleTapped
        {
            get { return _peopleTapped ?? (_peopleTapped = new RelayCommand(PeopleTap)); }
        }
        public RelayCommand MoviesTapped
        {
            get { return _moviesTapped ?? (_moviesTapped = new RelayCommand(MoviesTap)); }
        }

        public string MoviesAmountFormatted
        {
            get { return string.Format("({0})", MovieSearchResultAmout); }
        }

        public string TvShowsAmountFormatted
        {
            get { return string.Format("({0})", ShowSearchResultAmout); }
        }

        public object UsersAmountFormatted
        {
            get { return string.Format("({0})", UserSearchResultAmout); }
        }

        public double OpacityMovies
        {
            get { return IsMoviesVisible ? 1 : 0.5; }
        }
        public double OpacityShows
        {
            get { return IsTvShowsVisible ? 1 : 0.5; }
        }
        public double OpacityUsers
        {
            get { return IsUserVisible ? 1 : 0.5; }
        }

        public RelayCommand<MiniShowDataModel> ShowsClicked
        {
            get { return _showsClicked ?? (_showsClicked = new RelayCommand<MiniShowDataModel>(ShowClick)); }
        }

        public RelayCommand<MiniMovieDataModel> MoviesClicked
        {
            get { return _moviesClicked ?? (_moviesClicked = new RelayCommand<MiniMovieDataModel>(MoviesClick)); }
        }

        public RelayCommand<UserDataModel> UsersClicked
        {
            get { return _usersClicked ?? (_usersClicked = new RelayCommand<UserDataModel>(UsersClick)); }
        }

        private void UsersClick(UserDataModel obj)
        {
            if (obj == null) return;
            App.RootFrame.Navigate(typeof(UserProfileView), obj.Username);
        }

        private async void MoviesClick(MiniMovieDataModel obj)
        {
            if (obj == null) return;
            obj.IsLoadingData = true;
            await CoreServices.Movie.SetCurrent(obj.ToModel());
            obj.IsLoadingData = false;
            App.RootFrame.Navigate(typeof(MoviePage2));
        }

        private async void ShowClick(MiniShowDataModel obj)
        {
            if (obj == null) return;
            obj.IsLoadingData = true;
            await CoreServices.Show.SetCurrent(obj.Model);
            obj.IsLoadingData = false;
            App.RootFrame.Navigate(typeof(SeriePage));
        }


        private void UserTap()
        {
            App.RootFrame.Navigate(typeof(UserProfileView), CurrentUserAccount.Username);
        }

        public async void ActivityClick(ActivityDataModel obj)
        {
            switch (obj.ActivityType)
            {
                case ActivityTypes.Episode:
                    var res = await CoreServices.Show.SetCurrent(obj.Show.ToModel());

                    if (res.IsOk)
                    {
                        var data = new EpisodeViewerDataModelMini(obj.Episode.Season, obj.Episode.Number);
                        App.RootFrame.Navigate(typeof(EpisodeViewer), JsonConvert.SerializeObject(data));
                    }
                    else
                    {
                        var msgDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorNavigateToShow_Capital"), ShiftvHelpers.GetTranslation("ErrorNavigateToShowTitle_Capital"));
                        msgDialog.ShowAsync();
                    }

                    break;
                case ActivityTypes.Show:
                    var resShow = await CoreServices.Show.SetCurrent(obj.Show.ToModel());
                    if (resShow.IsOk)
                    {
                        App.RootFrame.Navigate(typeof(SeriePage));
                    }
                    else
                    {
                        var msgDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorNavigateToShow_Capital"), ShiftvHelpers.GetTranslation("ErrorNavigateToShowTitle_Capital"));
                        msgDialog.ShowAsync();
                    }
                    break;
                case ActivityTypes.Movie:
                    var resMovie =  await CoreServices.Movie.SetCurrent(obj.Movie.ToModel());
                    if (resMovie.IsOk)
                    {
                        App.RootFrame.Navigate(typeof(MoviePage2));
                    }
                    else
                    {
                        var msgDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorNavigateToShow_Capital"), ShiftvHelpers.GetTranslation("ErrorNavigateToShowTitle_Capital"));
                        msgDialog.ShowAsync();
                    }
                    break;
                case ActivityTypes.Unknown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void UserCliked(ActivityDataModel activity)
        {
            if (activity == null) return;
            App.RootFrame.Navigate(typeof(UserProfileView), activity.User.Username);
            //App.RootFrame.Navigate(typeof(UserProfileView), "edumserrano");  
        }
    }
}

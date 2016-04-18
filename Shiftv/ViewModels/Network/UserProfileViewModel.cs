using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Newtonsoft.Json;
using Shiftv.Common;
using Shiftv.Contracts.Domain.Activity;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Contracts.Services;
using Shiftv.DataModel;
using Shiftv.Helpers;
using Shiftv.Views.Movies;
using Shiftv.Views.Shows;
using Shiftv.Views.Shows.Player;

namespace Shiftv.ViewModels.Network
{
    public class UserProfileViewModel : ViewModelBase
    {
        private UserDataModel _userProfile;
        private Thickness _margin;
        private ObservableCollection<ActivityDataModel> _activities;
        private ObservableCollection<ShowDataModel> _lovedShows;
        private bool _isLoadingLastest;
        private bool _isLoadingFavoriteShows;
        private string _lastestName;
        private string _uriLatest;
        private string _action;
        private RelayCommand<ActivityDataModel> _activityClicked;
        private bool _isLatestClicked;
        private RelayCommand<ShowDataModel> _showsClicked;
        private bool _isShowClicked;
        private ObservableCollection<MovieDataModel> _lovedMovies;
        private bool _isLoadingFavoriteMovies;
        private bool _noLovedShowssAvailable;
        private bool _noLovedMoviesAvailable;
        private bool _noActivitiesAvailable;
        private RelayCommand<MovieDataModel> _moviesClicked;
        private List<IUser> _friends ;
        private List<IUser> _followers;
        private List<IUser> _following;
        private string _friendStatusText;
        private IUserToken _currentLoggedUser;
        private RelayCommand _followClicked;
        private string _currentPageUsername;
        private RelayCommand _unfollowClicked;
        private bool _isNotDoingFollow;
        private bool _isNotDoingUnfollow;
        private bool _isWaitingApproval;

        public UserProfileViewModel()
        {
            if (DesignMode.DesignModeEnabled) LoadUserData("amiguinho");
        }

        public async void LoadUserData(string username)
        {
            _currentPageUsername = username;
            Action = ShiftvHelpers.GetTranslation("JustSeen_Upper");
            IsLoadingLastest = true;
            IsLoadingFavoriteShows = true;
            IsLoadingFavoriteMovies = true;
            if (username == null) username = CoreServices.User.GetCurrentUser() != null ? CoreServices.User.GetCurrentUser().UserSettings.User.Username : null;
            if (username == null) return;
            var userProf = await CoreServices.User.GetUserProfileByUsername(username);
            if (userProf.IsOk)
            {
                UserProfile = new UserDataModel(userProf.Data);
            }
            await LoadActivities(username);
            SetMainActivity();
            await LoadLovedShows(username);
            await LoadLovedMovies(username);
            var currentUser = CoreServices.User.GetCurrentUser();
            if(currentUser != null) await GetCurrentFriends(username);
        }

        private async Task GetCurrentFriends(string username)
        {
            _currentLoggedUser =  CoreServices.User.GetCurrentUser();
            if (username == _currentLoggedUser.UserSettings.User.Username) return;
            var friends = await CoreServices.User.GetFriendsByUsername(_currentLoggedUser.UserSettings.User.Username);
            var followers = await CoreServices.User.GetFollowersByUsername(_currentLoggedUser.UserSettings.User.Username);
            if (followers.IsOk)
            {
                _followers = followers.Data;
                if (_followers.Any(x => x.Username.ToLower() == _currentPageUsername.ToLower()))
                {
                    FriendStatusText = ShiftvHelpers.GetTranslation("IsFollowingYou");
                }
            }
            var following = await CoreServices.User.GetFollowingByUsername(_currentLoggedUser.UserSettings.User.Username);
            if (following.IsOk)
            {
                _following = following.Data;
                if (_following.Any(x => x.Username.ToLower() == _currentPageUsername.ToLower()))
                {
                    FriendStatusText = ShiftvHelpers.GetTranslation("IsFollowUser");
                  
                }
            }
            if (friends.IsOk)
            {
                _friends = friends.Data;
                if (_friends.Any(x => x.Username.ToLower() == _currentPageUsername.ToLower()))
                {
                    FriendStatusText = ShiftvHelpers.GetTranslation("IsFriend");
                }
            }
            if (IsWaitingApproval)
            {
                FriendStatusText = ShiftvHelpers.GetTranslation("WaitingForApproval");
            }
            RefreshFriendState();
        }

        private void RefreshFriendState()
        {
            OnPropertyChanged("IsInNetwork");
            OnPropertyChanged("CanUnFollow");
            OnPropertyChanged("CanFollow");
            OnPropertyChanged("FriendStatusText");
            IsNotDoingFollow = true;
            IsNotDoingUnfollow = true;
        }

        private async Task LoadLovedMovies(string username)
        {
            var lovedMovies = await CoreServices.Movie.GetLovedByUser(username);
            if (lovedMovies.IsOk)
            {
                foreach (var movies in lovedMovies.Data.Take(15))
                {
                    LovedMovies.Add(new MovieDataModel(movies));
                }
            }
            IsLoadingFavoriteMovies = false;
            NoLovedMoviesAvailable = LovedMovies.Count == 0;
        }

        private async Task LoadLovedShows(string username)
        {
            var lovedShows = await CoreServices.Show.GetLovedByUser(username);
            if (lovedShows.IsOk)
            {
                foreach (var shows in lovedShows.Data.Take(15))
                {
                    LovedShows.Add(new ShowDataModel(shows));
                }
            }
            IsLoadingFavoriteShows = false;
            NoLovedShowssAvailable = LovedShows.Count == 0;
        }

        private void SetMainActivity()
        {
            if (string.IsNullOrEmpty(LatestName))
            {
                if (Activities.Count > 0)
                {
                    var act0 = Activities[0];
                    if (act0.ActivityType == ActivityTypes.Episode || act0.ActivityType == ActivityTypes.Show)
                    {
                        LatestName = act0.Show.Title;
                        UriLatest = act0.Show.Image.Fanart.Full;
                    }
                    else
                    {
                        LatestName = Activities[0].Movie.Title;
                        UriLatest = Activities[0].Movie.Image.Fanart.Full;
                    }
                    Action = act0.TextAction.ToUpper();
                }
            }
        }

        private async Task LoadActivities(string username)
        {
            var activities = await CoreServices.Activity.GetUserActivity(username);
            if (activities.IsOk)
            {
                foreach (var actitityItem in activities.Data.ActivityItems.Take(15))
                {
                    Activities.Add(new ActivityDataModel(actitityItem));
                }
            }
            IsLoadingLastest = false;
            NoActivitiesAvailable = Activities.Count == 0;
        }


        public UserDataModel UserProfile
        {
            get { return _userProfile; }
            set
            {
                SetProperty(ref _userProfile, value);
                if (value != null) UpdateData();
            }
        }

        private void UpdateData()
        {
            OnPropertyChanged("Aka");
            OnPropertyChanged("Username");
            OnPropertyChanged("UriLatest");
            OnPropertyChanged("Avatar");
            OnPropertyChanged("FullName");
            OnPropertyChanged("Location");
            OnPropertyChanged("Gender");
            OnPropertyChanged("About");
            OnPropertyChanged("Age");
            OnPropertyChanged("IsVip");
            OnPropertyChanged("IsMale");
            OnPropertyChanged("IsFemale");
            OnPropertyChanged("LatestName");
            OnPropertyChanged("Info");
            OnPropertyChanged("WatchedEpisodes");
            OnPropertyChanged("WatchedShows");
            OnPropertyChanged("WatchedMovies");
            OnPropertyChanged("TotalFriends");
            OnPropertyChanged("TotalLovedEpisodes");
            OnPropertyChanged("TotalLovedShows");
            OnPropertyChanged("TotalLovedMovies");
            OnPropertyChanged("TotalShoutsEpisodes");
            OnPropertyChanged("TotalShoutsShows");
            OnPropertyChanged("TotalShoutsMovies");
            OnPropertyChanged("IsProtected");
        }


        public Thickness MarginTopHeight
        {
            get { return _margin; }
            set
            {

                SetProperty(ref _margin, value);
            }
        }

        public ObservableCollection<ActivityDataModel> Activities
        {
            get { return _activities ?? (_activities = new ObservableCollection<ActivityDataModel>()); }
        }

        public ObservableCollection<ShowDataModel> LovedShows
        {
            get { return _lovedShows ?? (_lovedShows = new ObservableCollection<ShowDataModel>()); }
        }
        public ObservableCollection<MovieDataModel> LovedMovies
        {
            get { return _lovedMovies ?? (_lovedMovies = new ObservableCollection<MovieDataModel>()); }
        }

        //public string Aka
        //{
        //    get { return UserProfile != null ? UserProfile.Aka : ShiftvHelpers.LargeTextPlaceHolder; }
        //}

        public string UriLatest
        {
            get { return string.IsNullOrEmpty(_uriLatest) ? UserProfile != null ? _currentLoggedUser.UserSettings.Account.CoverImage : ShiftvHelpers.UriImagePlaceHolder : _uriLatest; }
            set
            {
                SetProperty(ref _uriLatest, value);
            }
        }

        public string Avatar { get { return UserProfile != null ? UserProfile.Avatar : ShiftvHelpers.UriAvatarPlaceHolder; } }

        public string Username { get { return UserProfile != null ? UserProfile.Username : ShiftvHelpers.TextPlaceHolder; } }
        public string FullName { get { return UserProfile != null ? UserProfile.FullName : ShiftvHelpers.TextPlaceHolder; } }
        public string Location { get { return UserProfile != null ? UserProfile.Location : ShiftvHelpers.TextPlaceHolder; } }
        public string Gender { get { return UserProfile != null ? UserProfile.Gender : ShiftvHelpers.TextPlaceHolder; } }
        public string About { get { return UserProfile != null ? UserProfile.About : ShiftvHelpers.TextPlaceHolder; } }
        public string Age { get { return UserProfile != null ? UserProfile.Avatar : ShiftvHelpers.TextPlaceHolder; } }
        public bool IsVip { get { return UserProfile != null && UserProfile.IsVip; } }

        public bool IsMale { get { return UserProfile != null && UserProfile.IsMale; } }
        public bool IsFemale { get { return UserProfile != null && UserProfile.IsFemale; } }

        public string LatestName
        {
            get { return string.IsNullOrEmpty(_lastestName) ? UserProfile != null ? UserProfile.FullName : ShiftvHelpers.LargeTextPlaceHolder : _lastestName; }
            set
            {
                SetProperty(ref _lastestName, value);
            }
        }
        //public string Info { get { return UserProfile != null ? UserProfile.Info : ShiftvHelpers.TextPlaceHolder; } }

        public bool IsLoadingLastest
        {
            get { return _isLoadingLastest; }
            set { SetProperty(ref _isLoadingLastest, value); }
        }

        public bool IsLoadingFavoriteShows
        {
            get { return _isLoadingFavoriteShows; }
            set { SetProperty(ref _isLoadingFavoriteShows, value); }
        }   
        
        public bool IsLoadingFavoriteMovies
        {
            get { return _isLoadingFavoriteMovies; }
            set { SetProperty(ref _isLoadingFavoriteMovies, value); }
        }

        public string Action
        {
            get { return _action; }
            set { SetProperty(ref _action, value); }
        }
        public RelayCommand<ActivityDataModel> ActivityClicked
        {
            get { return _activityClicked ?? (_activityClicked = new RelayCommand<ActivityDataModel>(ActivityClick)); }
        }

        public bool IsLatestClicked
        {
            get { return _isLatestClicked; }
            set { SetProperty(ref _isLatestClicked, value); }
        }

        public bool IsShowClicked
        {
            get { return _isShowClicked; }
            set { SetProperty(ref _isShowClicked, value); }
        }

        public RelayCommand<ShowDataModel> ShowsClicked
        {
            get { return _showsClicked ?? (_showsClicked = new RelayCommand<ShowDataModel>(ShowClick)); }
        }

        //public string WatchedEpisodes
        //{
        //    get { return UserProfile != null ? UserProfile.TotalWatchedEpisodes : ShiftvHelpers.NumberPlaceHolder; }
        //}

        //public string WatchedShows
        //{
        //    get { return UserProfile != null ? UserProfile.TotalWatchedShows : ShiftvHelpers.NumberPlaceHolder; }
        //}

        //public string WatchedMovies
        //{
        //    get { return UserProfile != null ? UserProfile.TotalWatchedMovies : ShiftvHelpers.NumberPlaceHolder; }
        //}

        //public string TotalFriends
        //{
        //    get { return UserProfile != null ? UserProfile.TotalFriends : ShiftvHelpers.NumberPlaceHolder; }
        //}

        //public string TotalLovedEpisodes
        //{
        //    get { return UserProfile != null ? UserProfile.TotalLovedEpisodes : ShiftvHelpers.NumberPlaceHolder; }
        //}

        //public string TotalLovedShows
        //{
        //    get { return UserProfile != null ? UserProfile.TotalLovedShows : ShiftvHelpers.NumberPlaceHolder; }
        //}

        //public string TotalLovedMovies
        //{
        //    get { return UserProfile != null ? UserProfile.TotalLovedMovies : ShiftvHelpers.NumberPlaceHolder; }
        //}

        //public string TotalHatedEpisodes
        //{
        //    get { return UserProfile != null ? UserProfile.TotalHatedEpisodes : ShiftvHelpers.NumberPlaceHolder; }
        //}

        //public string TotalHatedShows
        //{
        //    get { return UserProfile != null ? UserProfile.TotalHatedShows : ShiftvHelpers.NumberPlaceHolder; }
        //}

        //public string TotalHatedMovies
        //{
        //    get { return UserProfile != null ? UserProfile.TotalHatedMovies : ShiftvHelpers.NumberPlaceHolder; }
        //}    
        
        //public string TotalShoutsEpisodes
        //{
        //    get { return UserProfile != null ? UserProfile.TotalShoutsEpisodes : ShiftvHelpers.NumberPlaceHolder; }
        //}

        //public string TotalShoutsShows
        //{
        //    get { return UserProfile != null ? UserProfile.TotalShoutsShows : ShiftvHelpers.NumberPlaceHolder; }
        //}

        //public string TotalShoutsMovies
        //{
        //    get { return UserProfile != null ? UserProfile.TotalShoutsMovies : ShiftvHelpers.NumberPlaceHolder; }
        //}

        public bool IsProtected
        {
            get { return UserProfile != null && UserProfile.IsProtected; }
        }

        public bool NoLovedMoviesAvailable
        {
            get { return _noLovedMoviesAvailable; }
            set { SetProperty(ref _noLovedMoviesAvailable, value); }
        }

        public bool NoLovedShowssAvailable
        {
            get { return _noLovedShowssAvailable; }
            set { SetProperty(ref _noLovedShowssAvailable, value); }
        }   
        
        public bool NoActivitiesAvailable
        {
            get { return _noActivitiesAvailable; }
            set { SetProperty(ref _noActivitiesAvailable, value); }
        }

        public RelayCommand<MovieDataModel> MoviesClicked
        {
            get { return _moviesClicked ?? (_moviesClicked = new RelayCommand<MovieDataModel>(MoviesClick)); }
        }

        public string FriendStatusText
        {
            get { return _friendStatusText; }
            set { SetProperty(ref _friendStatusText, value); }
        }

        public bool IsInNetwork
        {
            get
            {
                if (_friends == null) return false;
                return _friends.Any(x => x.Username.ToLower() == _currentPageUsername.ToLower()) ||
                       _followers.Any(x => x.Username.ToLower() == _currentPageUsername.ToLower()) ||
                       _following.Any(x => x.Username.ToLower() == _currentPageUsername.ToLower());
            }
        }

        public RelayCommand FollowClicked
        {
            get { return _followClicked ?? (_followClicked = new RelayCommand(FollowClick)); }
        }

        public RelayCommand UnFollowClicked
        {
            get { return _unfollowClicked ?? (_unfollowClicked = new RelayCommand(UnfollowClick)); }
        }

        public bool CanFollow
        {
            get
            {
                if (!IsInNetwork) return true;
                if (_following.Any(x => x.Username.ToLower() == _currentPageUsername.ToLower()) ||
                    _friends.Any(x => x.Username.ToLower() == _currentPageUsername.ToLower())) return false;
                return true;
            }
        }

        public bool CanUnFollow
        {
            get
            {
                if (!IsInNetwork) return false;
                if (_following.Any(x => x.Username.ToLower() == _currentPageUsername.ToLower()) ||
                    _friends.Any(x => x.Username.ToLower() == _currentPageUsername.ToLower())) return true;
                return false;
            }
        }

        public bool IsNotDoingFollow   
        {
            get { return _isNotDoingFollow; }
            set { SetProperty(ref _isNotDoingFollow,value); }
        }  
        public bool IsNotDoingUnfollow   
        {
            get { return _isNotDoingUnfollow; }
            set { SetProperty(ref _isNotDoingUnfollow, value); }
        }

        public bool IsWaitingApproval
        {
            get { return _isWaitingApproval; }
            set { SetProperty(ref _isWaitingApproval, value); }
        }

        private async void UnfollowClick()
        {
            IsNotDoingUnfollow = false;
            var unfollow = await CoreServices.Network.Unfollow(UserProfile.Username);
            if (unfollow.IsOk)
            {
                await GetCurrentFriends(_currentPageUsername);
            }
            IsNotDoingUnfollow = true;
        }

        private async void FollowClick()
        {
            IsNotDoingFollow = false;
            var follow = await CoreServices.Network.Follow(UserProfile.Username);
            if (follow.IsOk)
            {
                if (follow.Data.IsPending)
                {
                    FriendStatusText = ShiftvHelpers.GetTranslation("WaitingForApproval"); 
                }
                else
                {
                    if (follow.Data.IsPending) IsWaitingApproval = true;
                    await GetCurrentFriends(_currentPageUsername);
                }
            }
            IsNotDoingFollow = true;
        }


        private async void MoviesClick(MovieDataModel obj)
        {
            await CoreServices.Movie.SetCurrent(obj.ToModel());
            App.RootFrame.Navigate(typeof(MoviePage2));
           // NavigationService.NavigateTo(ViewModels.CustomerDetails);
        }

        private async void ShowClick(ShowDataModel showDataModel)
        {
            IsShowClicked = true;
            await CoreServices.Show.SetCurrent(showDataModel.ToModel());
            App.RootFrame.Navigate(typeof(SeriePage));
        }

        private async void ActivityClick(ActivityDataModel obj)
        {
            IsLatestClicked = true;
            switch (obj.ActivityType)
            {

                case ActivityTypes.Episode:
                    await CoreServices.Show.SetCurrent(obj.Show.ToModel());
                    var data = new EpisodeViewerDataModelMini(obj.Episode.Season, obj.Episode.Number);
                    App.RootFrame.Navigate(typeof(EpisodeViewer), JsonConvert.SerializeObject(data));
                    break;
                case ActivityTypes.Show:
                    await CoreServices.Show.SetCurrent(obj.Show.ToModel());
                    App.RootFrame.Navigate(typeof(SeriePage));
                    break;
                case ActivityTypes.Movie:
                    await CoreServices.Movie.SetCurrent(obj.Movie.ToModel());
                    App.RootFrame.Navigate(typeof(MoviePage2));
                    break;
                case ActivityTypes.Unknown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

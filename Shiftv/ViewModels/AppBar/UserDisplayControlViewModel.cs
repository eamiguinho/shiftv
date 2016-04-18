using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel;
using Windows.Storage;
using Newtonsoft.Json;
using Shiftv.Common;
using Shiftv.Contracts.Domain.Activity;
using Shiftv.Contracts.Services;
using Shiftv.DataModel;
using Shiftv.Helpers;
using Shiftv.Views;
using Shiftv.Views.Movies;
using Shiftv.Views.Network;
using Shiftv.Views.Shows;
using Shiftv.Views.Shows.Player;

namespace Shiftv.ViewModels.AppBar
{
    public class UserDisplayControlViewModel : ViewModelBase
    {
        private UserDataModel _currentUserAccount;
        private MultiObservableCollection<ActivityDataModel> _activities;
        private RelayCommand<ActivityDataModel> _activityClicked;
        private ObservableCollection<UserDataModel> _networkRequests;
        private bool _isLoadingData;
        private RelayCommand _userTapped;
        private SearchDisplayControlViewModel _searchViewModel;
        private string _username;
        private string _password;
        private bool _errorMessage;
        private RelayCommand _loginButtonPressed;
        private bool _isLoading;
        private MultiObservableCollection<ActivityDataModel> _activities1;
        private RelayCommand _logoutClicked;
        private RelayCommand _registerPressed;

        public UserDisplayControlViewModel()
        {
            if (DesignMode.DesignModeEnabled) LoadData();
            
        }


        public async void LoadData()
        {
            IsLoadingData = true;
            var user = CoreServices.User.GetCurrentUser();
            if (user != null) CurrentUserAccount = new UserDataModel(user.UserSettings.User);
            var act = await CoreServices.Activity.GetFriendsActivity();          
     
            if (act.IsOk)
            {
                var listActivities = new List<ActivityDataModel>();
                foreach (var activity in act.Data.ActivityItems)
                    if (listActivities.All(x => x.Id != activity.Id)) listActivities.Add(new ActivityDataModel(activity));
                Activities = new MultiObservableCollection<ActivityDataModel>(listActivities.Take(10), listActivities);
            }
            IsLoadingData = false;
            //var network = await CoreServices.Network.GetRequests();
            //foreach (var req in network.Data)
            //{
            //    NetworkRequests.Add(new UserDataModel(req));
            //}
        }

        public SearchDisplayControlViewModel SearchViewModel
        {
            get { return _searchViewModel ?? (_searchViewModel = new SearchDisplayControlViewModel()); }
        }

        public UserDataModel CurrentUserAccount
        {
            get { return _currentUserAccount; }
            set { SetProperty(ref _currentUserAccount, value); }
        }

        public MultiObservableCollection<ActivityDataModel> Activities
        {
            get { return _activities1; }
            set { SetProperty(ref _activities1, value); }
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
            get { return Activities != null && Activities.Count == 0 && IsLoadingData == false; }
        }

        public bool IsLoadingData
        {
            get { return _isLoadingData; }
            set
            {
                SetProperty(ref _isLoadingData, value);
                OnPropertyChanged("NoFriendActivities");
            }
        }

        public RelayCommand UserTapped
        {
            get { return _userTapped ?? (_userTapped = new RelayCommand(UserTap)); }
        }

        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public bool ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(ref _errorMessage, value); }
        }

        public RelayCommand LoginButtonPressed
        {
            get { return _loginButtonPressed ?? (_loginButtonPressed = new RelayCommand(Login)); }
        }

        private async void Login()
        {
            //ErrorMessage = false;
            //IsLoading = true;
            //var passwordSHA1 = ShiftvHelpers.SHA1Converter(Password);
            //var x = await CoreServices.User.LoginToTrakt(Username, passwordSHA1);
            //if (!x.IsOk) ErrorMessage = true;
            //else
            //{
            //    var a = await CoreServices.User.GetUser(Username, passwordSHA1);
            //    if (a.IsOk)
            //    {
            //        var localSettings = ApplicationData.Current.LocalSettings;
            //        localSettings.Values["Username"] = Username;
            //        localSettings.Values["Password"] = passwordSHA1;
            //        CoreServices.User.SetUser(a.Data);
            //        var currentPage = App.RootFrame.CurrentSourcePageType;
            //        App.RootFrame.Navigate(currentPage);
            //    }
            //    else ErrorMessage = true;
            //}
            //IsLoading = false;
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        public RelayCommand LogoutClicked
        {
            get { return _logoutClicked ?? (_logoutClicked = new RelayCommand(Logout)); }
        }

        public RelayCommand RegisterPressed
        {
            get { return _registerPressed ?? (_registerPressed = new RelayCommand(Register)); }
        }

        private void Register()
        {
            App.RootFrame.Navigate(typeof (LoginPage));
        }

        private void Logout()
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values.Remove("Username");
            localSettings.Values.Remove("Password");
            CoreServices.User.SetUser(null);
            CoreServices.Show.ClearTrending();
            CoreServices.Movie.ClearTrending();
            App.RootFrame.Navigate(typeof(LoginPage));
        }

        private void UserTap()
        {
            App.RootFrame.Navigate(typeof(UserProfileView), CurrentUserAccount.Username);
        }

        public async void ActivityClick(ActivityDataModel obj)
        {
            obj.IsLoadingData = true;
            switch (obj.ActivityType)
            {
                case ActivityTypes.Episode:
                    await CoreServices.Show.SetCurrent(obj.Show.ToModel());
                    var data = new EpisodeViewerDataModelMini(obj.Episode.Season, obj.Episode.Number);
                    obj.IsLoadingData = false;
                    App.RootFrame.Navigate(typeof(EpisodeViewer), JsonConvert.SerializeObject(data));
                    break;
                case ActivityTypes.Show:
                    await CoreServices.Show.SetCurrent(obj.Show.ToModel());
                    obj.IsLoadingData = false;
                    App.RootFrame.Navigate(typeof(SeriePage));
                    break;
                case ActivityTypes.Movie:
                    await CoreServices.Movie.SetCurrent(obj.Movie.ToModel());
                    obj.IsLoadingData = false;
                    App.RootFrame.Navigate(typeof(MoviePage2));
                    break;
                case ActivityTypes.Unknown:
                    obj.IsLoadingData = false;
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

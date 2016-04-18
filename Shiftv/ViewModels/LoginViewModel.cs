using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Http.Filters;
using Autofac;
using Newtonsoft.Json;
using Shiftv.Common;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.Domain.Results;
using Shiftv.Contracts.Services;
using Shiftv.Contracts.Services.Sync;
using Shiftv.Global;
using Shiftv.Helpers;

namespace Shiftv.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private RelayCommand _loginPressed;
        private bool _isLoading;
        private bool _errorMessage;
        private string _welcome;
        private BitmapImage _image1;
        private BitmapImage _image4;
        private BitmapImage _image3;
        private BitmapImage _image2;
        private BitmapImage _image5;
        public Random A = new Random(DateTime.Now.Ticks.GetHashCode());
        public List<int> RandomList = new List<int>();
        private string _createUsername;
        private string _createPassword;
        private string _createEmail;
        private RelayCommand _createPressed;
        private bool _createErrorMessage;
        private string _createErrorMessageText;
        private bool _canImageShow;
        private bool _canImageMovie;
        private RelayCommand _enterWithoutLogin;
        private string _errorMessageLogin;
        private Uri _sourceRequest;
        private bool _isWebviewVisible;
        private RelayCommand _enterWithLogin;


        public LoginViewModel()
        {
            LoadImages();
        }

        private async void LoadImages()
        {
            App.LoginVm = this;
            IsWebviewVisible = true;
            SourceRequest =
                new Uri("https://trakt.tv/oauth/authorize?response_type=code&client_id=233fcb9838282957f4d5b6f4fdd7d0167bb8344bcd2463eaaa9cfc4a659da9b5&redirect_uri=http://shiftvapi.azurewebsites.net");
            Image5 = await ImageHelper.GetShowImageInCache();
            Image4 = await ImageHelper.GetMovieImageInCache();
            var showsReq = await CoreServices.Show.GetTrending(true);
            var moviesReq = await CoreServices.Movie.GetTrending(true);
            _canImageShow = showsReq.Result == StandardResults.Ok && showsReq.Data != null && showsReq.Data.Count > 10;
            _canImageMovie = moviesReq.Result == StandardResults.Ok && moviesReq.Data != null && moviesReq.Data.Count > 10;
            RandomList.Clear();
            if (_canImageShow) Image1 = await ImageHelper.GetShowImageAsync(new Uri(showsReq.Data[NewNumber()].Fanart.Full));
            if (_canImageShow) Image3 = await ImageHelper.GetShowImageAsync(new Uri(showsReq.Data[NewNumber()].Fanart.Full));
            if (Image5 == null && _canImageShow) Image5 = await ImageHelper.GetShowImageAsync(new Uri(showsReq.Data[NewNumber()].Fanart.Full));
            RandomList.Clear();
            if (_canImageMovie) Image2 = await ImageHelper.GetMovieImageAsync(new Uri(moviesReq.Data[NewNumber()].Fanart.Full));
            if (Image4 == null && _canImageMovie) Image4 = await ImageHelper.GetMovieImageAsync(new Uri(moviesReq.Data[NewNumber()].Fanart.Full));

        }



        private int NewNumber()
        {
            var mynumber = A.Next(0, 10);
            while (RandomList.Contains(mynumber))
            {
                mynumber = A.Next(0, 10);
            }
            RandomList.Add(mynumber);
            return mynumber;
        }


        public string Username { get; set; }
        public string Password { get; set; }

        public RelayCommand LoginButtonPressed
        {
            get { return _loginPressed ?? (_loginPressed = new RelayCommand(LoginButtonPressedCommand)); }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                SetProperty(ref _isLoading, value);

            }
        }

        public bool ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(ref _errorMessage, value); }
        }
        public string ErrorMessageLogin
        {
            get { return _errorMessageLogin; }
            set { SetProperty(ref _errorMessageLogin, value); }
        }

        public string Welcome
        {
            get { return _welcome; }
            set { SetProperty(ref _welcome, value); }
        }

        public BitmapImage Image1
        {
            get { return _image1; }
            set { SetProperty(ref _image1, value); }
        }
        public BitmapImage Image2
        {
            get { return _image2; }
            set { SetProperty(ref _image2, value); }
        }
        public BitmapImage Image3
        {
            get { return _image3; }
            set { SetProperty(ref _image3, value); }
        }
        public BitmapImage Image4
        {
            get { return _image4; }
            set { SetProperty(ref _image4, value); }
        }
        public BitmapImage Image5
        {
            get { return _image5; }
            set { SetProperty(ref _image5, value); }
        }

        public string CreateUsername
        {
            get { return _createUsername; }
            set { SetProperty(ref _createUsername, value); }
        }

        public string CreatePassword
        {
            get { return _createPassword; }
            set { SetProperty(ref _createPassword, value); }
        }

        public string CreateEmail
        {
            get { return _createEmail; }
            set { SetProperty(ref _createEmail, value); }
        }

        public RelayCommand CreateAccountButtonPressed
        {
            get { return _createPressed ?? (_createPressed = new RelayCommand(CreateAccount)); }
        }

        public bool CreateErrorMessage
        {
            get { return _createErrorMessage; }
            set { SetProperty(ref _createErrorMessage, value); }
        }

        public string CreateErrorMessageText
        {
            get { return _createErrorMessageText; }
            set { SetProperty(ref _createErrorMessageText, value); }
        }

        public RelayCommand EnterWithoutLogin
        {
            get { return _enterWithoutLogin ?? (_enterWithoutLogin = new RelayCommand(EnterWithoutLoginAction)); }
        }

        public Uri SourceRequest
        {
            get { return _sourceRequest; }
            set { SetProperty(ref _sourceRequest, value); }
        }

        public bool IsWebviewVisible
        {
            get { return _isWebviewVisible; }
            set { SetProperty(ref _isWebviewVisible, value); }
        }

        public RelayCommand EnterWithLogin
        {
            get { return _enterWithLogin ?? (_enterWithLogin = new RelayCommand(OpenLoginBrowser)); }
        }
        public static void ClearCookies(string url)
        {
            var myFilter = new HttpBaseProtocolFilter();
            var cookieManager = myFilter.CookieManager;
            var myCookieJar = cookieManager.GetCookies(new Uri(url));
            foreach (var cookie in myCookieJar)
            {
                cookieManager.DeleteCookie(cookie);
            }
        }
        private void OpenLoginBrowser()
        {
            ClearCookies("https://trakt.tv/oauth/authorize");
            Windows.System.Launcher.LaunchUriAsync(
                new Uri(
                    "https://trakt.tv/oauth/authorize?response_type=code&client_id=233fcb9838282957f4d5b6f4fdd7d0167bb8344bcd2463eaaa9cfc4a659da9b5&redirect_uri=shiftv://login.com"));
        }

        private void EnterWithoutLoginAction()
        {
            Welcome = string.Format("{0} {1}! {2} :)", ShiftvHelpers.GetTranslation("Hi_Upper"), ShiftvHelpers.GetTranslation("Unknown_Upper"), ShiftvHelpers.GetTranslation("Enjoy_Upper"));
        }

        private async void CreateAccount()
        {
            //CreateErrorMessage = false;
            //IsLoading = true;
            //var passwordSHA1 = ShiftvHelpers.SHA1Converter(CreatePassword);
            //var x = await CoreServices.User.CreateAccount(CreateUsername, passwordSHA1, CreateEmail);
            //if (!x.IsOk)
            //{
            //    CreateErrorMessageText = x.ErrorMessage;
            //    CreateErrorMessage = true;
            //}
            //else
            //{
            //    var a = await CoreServices.User.GetUser(CreateUsername, passwordSHA1);
            //    if (a.IsOk)
            //    {
            //        var localSettings = ApplicationData.Current.LocalSettings;
            //        localSettings.Values["Username"] = CreateUsername.ToLower().Trim();
            //        localSettings.Values["Password"] = passwordSHA1;
            //        CoreServices.User.SetUser(a.Data);
            //        Welcome = string.Format("{0} {1}! {2} :)", ShiftvHelpers.GetTranslation("Hi_Upper"), string.IsNullOrEmpty(a.Data.UserProfile.FullName) ? CreateUsername.ToUpper() : a.Data.UserProfile.FullName.ToUpper(), ShiftvHelpers.GetTranslation("Enjoy_Upper"));
            //    }
            //    else CreateErrorMessage = true;
            //}
            //IsLoading = false;
        }


        public async void LoginButtonPressedCommand()
        {
            ErrorMessage = false;
            IsLoading = true;
            if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Username))
            {
                ErrorMessageLogin = ShiftvHelpers.GetTranslation("UsernamePasswordRequired");
                ErrorMessage = true;
                IsLoading = false;
                return;
            }
            var passwordSHA1 = ShiftvHelpers.SHA1Converter(Password);
            var x = await CoreServices.User.LoginToTrakt(Username, passwordSHA1);
            if (!x.IsOk)
            {
                switch (x.Result)
                {
                    case ResultBase.Results.Ok:
                        break;
                    case ResultBase.Results.Error:
                        var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("FirstLoginAttemptMessage_Capital"), ShiftvHelpers.GetTranslation("FirstLoginAttemptTitle_Capital"));
                        messageDialog.ShowAsync();
                        break;
                    case ResultBase.Results.NoInternetConnection:
                        ErrorMessageLogin = ShiftvHelpers.GetTranslation("CheckInternetConnection");
                        break;
                    case ResultBase.Results.Unauthorized:
                        ErrorMessageLogin = ShiftvHelpers.GetTranslation("ErrorUsernamePassword");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                ErrorMessage = true;
            }
            else
            {
                //var a = await CoreServices.User.GetUser(Username, passwordSHA1);
                //if (a.IsOk)
                //{
                //    var localSettings = ApplicationData.Current.LocalSettings;
                //    localSettings.Values["Username"] = Username.ToLower().Trim();
                //    localSettings.Values["Password"] = passwordSHA1;
                //    CoreServices.User.SetUser(a.Data);
                //    Welcome = string.Format("{0} {1}! {2} :)", ShiftvHelpers.GetTranslation("Hi_Capital"), string.IsNullOrEmpty(a.Data.UserProfile.FullName) ? Username.ToUpper() : a.Data.UserProfile.FullName.ToUpper(), ShiftvHelpers.GetTranslation("Enjoy"));
                //}
                //else ErrorMessage = true;
            }
            IsLoading = false;

        }


        public async Task DoGetToken(string authCode)
        {
            IsWebviewVisible = false;   
            IsLoading = true;
            var res = await CoreServices.User.GetToken(authCode);
            if (res.IsOk && res.Data != null)
            {
               
                IsLoading = false;
                Welcome = string.Format("{0} {1}! {2} :)", ShiftvHelpers.GetTranslation("Hi_Capital"), string.IsNullOrEmpty(res.Data.UserSettings.User.Name) ? res.Data.UserSettings.User.Username.ToUpper() : res.Data.UserSettings.User.Name.ToUpper(), ShiftvHelpers.GetTranslation("Enjoy"));
                var localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values["userData"] = JsonConvert.SerializeObject(UserTokenDtoFactory.GetDto(res.Data));
                CoreServices.User.SetUser(res.Data);
            }
            else
            {
                IsWebviewVisible = true;
                IsLoading = false;
                var messageDialog = new MessageDialog(ShiftvHelpers.GetTranslation("FirstLoginAttemptMessage_Capital"), ShiftvHelpers.GetTranslation("FirstLoginAttemptTitle_Capital"));
                messageDialog.ShowAsync();
            }
        }

        public async Task SyncData()
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            if (!localSettings.Values.ContainsKey("firstTime"))
            {
                var syncService = Ioc.Container.Resolve<ISyncService>();
                var syncWatchedShows = await syncService.SyncWatchedShows();
                var syncWatchedMovies = await syncService.SyncWatchedMovies();
                var syncShowRatings = await syncService.SyncShowRatings();
                //var syncSeasonsRatings = await syncService.SyncSeasonRatings();
                var syncEpisodesRatings = await syncService.SyncEpisodeRatings();
                var syncMovieRatings = await syncService.SyncMovieRatings();
            }
            else
            {
                Task.Run(async () =>
                {
                    try
                    {
                        var syncService = Ioc.Container.Resolve<ISyncService>();
                        var uploadRatingsToTrakt = await syncService.UploadRatingsToTrakt();


                        var uploadWatchedEpisodesToTrakt = await syncService.SyncWatchedShows();
                        var uploadWatchedMoviesToTrakt = await syncService.SyncWatchedShows();
                        var syncWatchedShows = await syncService.SyncWatchedShows();
                        var syncWatchedMovies = await syncService.SyncWatchedMovies();
                        var syncShowRatings = await syncService.SyncShowRatings();
                        //var syncSeasonsRatings = await syncService.SyncSeasonRatings();
                        var syncEpisodesRatings = await syncService.SyncEpisodeRatings();
                        var syncMovieRatings = await syncService.SyncMovieRatings();
                    }
                    catch (Exception e)
                    {
                        
                    }
                });
            }
        }
    }
}

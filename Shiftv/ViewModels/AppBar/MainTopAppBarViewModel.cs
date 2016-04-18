using Windows.UI.Xaml.Media.Imaging;
using Shiftv.Common;
using Shiftv.Contracts.Data.Crawler;
using Shiftv.Contracts.Services;
using Shiftv.DataModel;
using Shiftv.Views.Movies.Pages;
using Shiftv.Views.OfflineContent;
using Shiftv.Views.Shows.Pages;

namespace Shiftv.ViewModels.AppBar
{
    public class MainTopAppBarViewModel : ViewModelBase
    {
        private RelayCommand _trendingClicked;
        private RelayCommand _recommendedClicked;
        private RelayCommand _myShowsClicked;
        private RelayCommand _calendarClicked;
        private RelayCommand _animeClicked;
        private RelayCommand _downloadManagerClicked;
        private RelayCommand _topClicked;
        private UserDataModel _currentUserAccount;
        private RelayCommand _freshClicked;
        private RelayCommand _topImdbClicked;
        private RelayCommand _dashboardClicked;
        private BitmapImage _movieImage;
        private RelayCommand _goToCinema;

        public MainTopAppBarViewModel()
        {
            var user = CoreServices.User.GetCurrentUser();
            if (user != null) CurrentUserAccount = new UserDataModel(user.UserSettings.User);
            LoadImage();
        }

        private async void LoadImage()
        {
            MovieImage = await Helpers.ImageHelper.GetMovieImageInCache();
        }

        public UserDataModel CurrentUserAccount
        {
            get { return _currentUserAccount; }
            set { SetProperty(ref _currentUserAccount, value); }
        }

        public RelayCommand TrendingClicked
        {
            get { return _trendingClicked ?? (_trendingClicked = new RelayCommand(TrendingClick)); }
        }

        public RelayCommand RecommendedClicked
        {
            get { return _recommendedClicked ?? (_recommendedClicked = new RelayCommand(RecommendedClick)); }
        }

        public RelayCommand MyShowsClicked
        {
            get { return _myShowsClicked ?? (_myShowsClicked = new RelayCommand(MyShowsClick)); }
        }

        public RelayCommand CalendarClicked
        {
            get { return _calendarClicked ?? (_calendarClicked = new RelayCommand(CalendarClick)); }
        }

        public RelayCommand AnimeClicked
        {
            get { return _animeClicked ?? (_animeClicked = new RelayCommand(AnimeClick)); }
        }

        public RelayCommand DownloadManagerClicked
        {
            get { return _downloadManagerClicked ?? (_downloadManagerClicked = new RelayCommand(DownloadManagerClick)); }
        }

        public RelayCommand TopClicked
        {
            get { return _topClicked ?? (_topClicked = new RelayCommand(TopClick)); }
        }

        public RelayCommand FreshClicked
        {
            get { return _freshClicked ?? (_freshClicked = new RelayCommand(FreshClick)); }
        }

        public RelayCommand TopImdbClicked
        {
            get { return _topImdbClicked ?? (_topImdbClicked = new RelayCommand(TopImdbClick)); }
        }

        public RelayCommand DashboardClicked
        {
            get { return _dashboardClicked ?? (_dashboardClicked = new RelayCommand(DashboardClick)); }
        }

        public BitmapImage MovieImage
        {
            get { return _movieImage; }
            set { SetProperty(ref _movieImage, value); }
        }

        public RelayCommand GoToCinema
        {
            get { return _goToCinema ?? (_goToCinema = new RelayCommand(GoToCinemaClick)); }
        }

        private void GoToCinemaClick()
        {
            App.RootFrame.Navigate(typeof(TrendingMovies));
        }

        private void DashboardClick()
        {
            if (!IsUserLogged) return;
            App.RootFrame.Navigate(typeof(DashboardPage));
        }

        private void TopImdbClick()
        {
            App.RootFrame.Navigate(typeof(TopImdbShowsPage));
        }

        private void FreshClick()
        {
            App.RootFrame.Navigate(typeof(FreshShowsPage));
        }

        private void TopClick()
        {
            App.RootFrame.Navigate(typeof(TopShowsPage));
        }


        private void DownloadManagerClick()
        {
            App.RootFrame.Navigate(typeof(OfflineContentManager));
        }

        private void AnimeClick()
        {
            App.RootFrame.Navigate(typeof(AnimePage));
        }

        public void TrendingClick()
        {
            App.RootFrame.Navigate(typeof(TrendingShowsPage));
        }

        public void CalendarClick()
        {
            if (!IsUserLogged) return;
            App.RootFrame.Navigate(typeof(Calendar));
        }

        public void MyShowsClick()
        {
            if (!IsUserLogged) return;
            App.RootFrame.Navigate(typeof(MyShowsPage));
        }

        public void RecommendedClick()
        {
            if (!IsUserLogged) return;
            App.RootFrame.Navigate(typeof(RecommendedShowsPage));
        }

    }
}

using Windows.UI.Xaml.Media.Imaging;
using Shiftv.Common;
using Shiftv.Contracts.Services;
using Shiftv.DataModel;
using Shiftv.Views.Movies;
using Shiftv.Views.Movies.Pages;
using Shiftv.Views.Shows.Pages;

namespace Shiftv.ViewModels.AppBar
{
    public class MoviesMainTopAppBarViewModel : ViewModelBase
    {
        private RelayCommand _trendingClicked;
        private RelayCommand _recommendedClicked;
        private RelayCommand _myShowsClicked;
        private RelayCommand _topClicked;
        private UserDataModel _currentUserAccount;
        private RelayCommand _freshClicked;
        private RelayCommand _topImdbClicked;
        private RelayCommand _animationClicked;
        private RelayCommand _watchlistClicked;
        private BitmapImage _showImage;
        private RelayCommand _goToTvShows;
        private RelayCommand _oscarClick;

        public MoviesMainTopAppBarViewModel()
        {
            var user = CoreServices.User.GetCurrentUser();
            if (user != null) CurrentUserAccount = new UserDataModel(user.UserSettings.User);
            LoadImage();
        }
        public UserDataModel CurrentUserAccount
        {
            get { return _currentUserAccount; }
            set { SetProperty(ref _currentUserAccount, value); }
        }

        private async void LoadImage()
        {
            ShowImage = await Helpers.ImageHelper.GetShowImageInCache();
        }


        public RelayCommand TrendingClicked
        {
            get { return _trendingClicked ?? (_trendingClicked = new RelayCommand(TrendingClick)); }
        }

        public RelayCommand RecommendedClicked
        {
            get { return _recommendedClicked ?? (_recommendedClicked = new RelayCommand(RecommendedClick)); }
        }

        public RelayCommand MyMoviesClicked
        {
            get { return _myShowsClicked ?? (_myShowsClicked = new RelayCommand(MyMoviesClick)); }
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

        public RelayCommand AnimationClicked
        {
            get { return _animationClicked ?? (_animationClicked = new RelayCommand(AnimationClick)); }
        }

        public RelayCommand WatchlistClicked
        {
            get { return _watchlistClicked ?? (_watchlistClicked = new RelayCommand(WatchlistClick)); }
        }

        public BitmapImage ShowImage
        {
            get { return _showImage; }
            set { SetProperty(ref _showImage, value); }
        }

        public RelayCommand GoToTvShows
        {
            get { return _goToTvShows ?? (_goToTvShows = new RelayCommand(GoToTvShowsClick)); }
        }

        public RelayCommand OscarsClicked
        {
            get { return _oscarClick ?? (_oscarClick = new RelayCommand(OscarsClick)); }
        }

        private void OscarsClick()
        {
            App.RootFrame.Navigate(typeof(OscarsMovies));
        }

        private void GoToTvShowsClick()
        {
            App.RootFrame.Navigate(typeof(TrendingShowsPage));
        }

        private void WatchlistClick()
        {
            App.RootFrame.Navigate(typeof(MyWatchlistedMovies));
        }

        private void AnimationClick()
        {
            App.RootFrame.Navigate(typeof(ChristmasMovies));
        }

        private void TopImdbClick()
        {
            App.RootFrame.Navigate(typeof(TopImdbMovies));
        }

        private void FreshClick()
        {
            App.RootFrame.Navigate(typeof(FreshMovies));
        }

        private void TopClick()
        {
            App.RootFrame.Navigate(typeof(TopMovies), false);
        }
        public void TrendingClick()
        {
            App.RootFrame.Navigate(typeof(TrendingMovies));
        }

        public void MyMoviesClick()
        {
            if (!IsUserLogged) return;
            App.RootFrame.Navigate(typeof(MyMovies));
        }

        public void RecommendedClick()
        {
            if (!IsUserLogged) return;
            App.RootFrame.Navigate(typeof(RecommendedMovies));
        }
    }
}

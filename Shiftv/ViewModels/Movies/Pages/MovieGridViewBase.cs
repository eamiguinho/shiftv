using System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight.Messaging;
using Shiftv.Common;
using Shiftv.Contracts.Services;
using Shiftv.DataModel;
using Shiftv.Helpers;
using Shiftv.Views.Movies;

namespace Shiftv.ViewModels.Movies.Pages
{
    public abstract class MovieGridViewBase : ViewModelBase
    {
        private bool _isDataLoaded;
        private double _itemHeight;
        private double _itemWidth;
        protected int NumberRequested;
        protected bool IsProcessing;
        private RelayCommand _retryClicked;
        private UserDataModel _currentUserAccount;
        private RelayCommand<MiniMovieDataModel> _movieClicked;
        private bool _addShowed;
        protected int _pageSize = -1;
        private RelayCommand _openTopAppBarClicked;
        private bool _isLeftBarsOpen;
        private RelayCommand _leftAppBarsBlockerClicked;
        private bool _isAppBarOpen;

        public MovieGridViewBase()
        {
           
        }
        public bool IsDataLoaded
        {
            get { return _isDataLoaded; }
            set { SetProperty(ref _isDataLoaded, value); }
        }
        public bool IsLeftBarsOpen
        {
            get { return _isLeftBarsOpen; }
            set { SetProperty(ref _isLeftBarsOpen, value); }
        }
        public double ItemWidth2
        {
            get
            {
                var bounds = Window.Current.Bounds;
                return bounds.Width / 3 - 4;
            }
        }
        public double ItemWidth
        {
            get { return _itemWidth; }
            set { SetProperty(ref _itemWidth, value); }
        }

        public double ItemHeight
        {
            get { return _itemHeight; }
            set { SetProperty(ref _itemHeight, value); }
        }

        public double ItemHeight2
        {
            get
            {
                var bounds = Window.Current.Bounds;
                return bounds.Height / 3 - 3.35;
            }
        }
        public double ItemHeightPub
        {
            get
            {
                var bounds = Window.Current.Bounds;
                return (bounds.Height / 3 - 3.35) - 10;
            }
        }

        public bool AddShowed
        {
            get { return _addShowed; }
            set { SetProperty(ref _addShowed, value); }
        }
        public RelayCommand<MiniMovieDataModel> MovieClicked
        {
            get { return _movieClicked ?? (_movieClicked = new RelayCommand<MiniMovieDataModel>(MovieClick)); }
        }

        public RelayCommand RetryClicked
        {
            get { return _retryClicked ?? (_retryClicked = new RelayCommand(LoadData)); }
        }
        public UserDataModel CurrentUserAccount
        {
            get { return _currentUserAccount; }
            set { SetProperty(ref _currentUserAccount, value); }
        }

        public RelayCommand OpenTopAppBarClicked
        {
            get { return _openTopAppBarClicked ?? (_openTopAppBarClicked = new RelayCommand(OpenTopAppBarClick)); }
        }

        private void OpenTopAppBarClick()
        {
            IsAppBarOpen = true;
        }

        public bool IsAppBarOpen
        {
            get { return _isAppBarOpen; }
            set { SetProperty(ref _isAppBarOpen, value); }
        }


        public bool IsClickSafe { get; set; }

        private void MovieClick(MiniMovieDataModel obj)
        {
            if (obj == null || obj.IsAdd) return;
            OpenMovieDetails(obj);
        }

        public async void OpenMovieDetails(MiniMovieDataModel serie)
        {
            if (IsClickSafe) return;
            IsClickSafe = true;
            serie.IsLoadingData = true;
            var res = await CoreServices.Movie.SetCurrent(serie.ToModel());
            if (res.IsOk)
            {
                App.RootFrame.Navigate(typeof(MoviePage2));
            }
            else
            {
                var msgDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorNavigateToShow_Capital"), ShiftvHelpers.GetTranslation("ErrorNavigateToShowTitle_Capital"));
                msgDialog.ShowAsync();
            }
            serie.IsLoadingData = false;
            serie.ImageOpacity = 1;
            IsClickSafe = false;
        }


        public void CalculateWidthHeight(double height, double width)
        {
            ItemHeight = height / 3 - 3.35;
            ItemWidth = width / 3 - 4;
        }
        public int PageSize
        {
            get
            {
                if (_pageSize == -1) _pageSize = IsToShowAds && !AddShowed ? 19 : 20;
                return _pageSize;
            }
        }
        public abstract void LoadData();
    }
}


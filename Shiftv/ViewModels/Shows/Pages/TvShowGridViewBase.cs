using System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight.Messaging;
using Shiftv.Common;
using Shiftv.Contracts.Services;
using Shiftv.DataModel;
using Shiftv.Helpers;
using Shiftv.Views.Shows;

namespace Shiftv.ViewModels.Shows.Pages
{
    public abstract class TvShowGridViewBase : ViewModelBase
    {
        private bool _isDataLoaded;
        private double _itemHeight;
        private double _itemWidth;
        protected int NumberRequested;
        protected bool IsProcessing;
        private RelayCommand _retryClicked;
        private RelayCommand<MiniShowDataModel> _showClicked;
        private UserDataModel _currentUserAccount;
        private bool _addShowed;
        protected int _pageSize = -1;
        private RelayCommand _openTopAppBarClicked;
        private bool _isAppBarOpen;
        private bool _isLeftBarsOpen;
        private RelayCommand _leftAppBarsBlockerClicked;

        protected TvShowGridViewBase()
        {
         
        }

        public bool IsDataLoaded
        {
            get { return _isDataLoaded; }
            set { SetProperty(ref _isDataLoaded,  value); }
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

        public bool AddShowed
        {
            get { return _addShowed; }
            set { SetProperty(ref _addShowed, value); }
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
        public RelayCommand<MiniShowDataModel> ShowClicked
        {
            get { return _showClicked ?? (_showClicked = new RelayCommand<MiniShowDataModel>(ShowClick)); }
        }
        public RelayCommand OpenTopAppBarClicked
        {
            get { return _openTopAppBarClicked ?? (_openTopAppBarClicked = new RelayCommand(OpenTopAppBarClick)); }
        }

        private void OpenTopAppBarClick()
        {
            IsAppBarOpen = true;
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

        public bool IsClickSafe { get; set; }

        public bool IsAppBarOpen
        {
            get { return _isAppBarOpen; }
            set { SetProperty(ref _isAppBarOpen, value); }
        }

        private void ShowClick(MiniShowDataModel obj)
        {
            if (obj == null || obj.IsAdd) return;
            OpenSerieDetails(obj);
        }

        public async void OpenSerieDetails(MiniShowDataModel serie)
        {
            if(IsClickSafe) return;
            IsClickSafe = true;
           // IsDataLoaded = false;
            serie.IsLoadingData = true;
            var res = await CoreServices.Show.SetCurrent(serie.Model);
            if (res.IsOk)
            {
                App.RootFrame.Navigate(typeof (SeriePage));
            }
            else
            {
                var msgDialog = new MessageDialog(ShiftvHelpers.GetTranslation("ErrorNavigateToShow_Capital"), ShiftvHelpers.GetTranslation("ErrorNavigateToShowTitle_Capital"));
                msgDialog.ShowAsync();
            }
           // IsDataLoaded = true;
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
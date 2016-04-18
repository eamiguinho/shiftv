using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Shiftv.Common;
using Shiftv.ViewModels;
using Shiftv.ViewModels.Shows.Pages;
using WinRTXamlToolkit.Controls.Extensions;

namespace Shiftv.Views.Shows.Pages
{
    public sealed partial class TrendingShowsPage
    {

        private readonly NavigationHelper _navigationHelper;
        private ScrollViewer _scrollViewer;

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return _navigationHelper; }
        }

        public TrendingShowsPageViewModel ViewModel { get { return (TrendingShowsPageViewModel)DataContext; } }


        public TrendingShowsPage()
        {
            InitializeComponent();
            _navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += navigationHelper_LoadState;
            _navigationHelper.SaveState += navigationHelper_SaveState;
           // NavigationCacheMode = NavigationCacheMode.Enabled;
            ListView.Loaded += ItemGridViewOnLoaded;
            CalculateHeight();
            NavigationCacheMode = NavigationCacheMode.Enabled;

        }

        private void ItemGridViewOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _scrollViewer = ListView.GetFirstDescendantOfType<ScrollViewer>();
            _scrollViewer.ViewChanged += scrollViewer_ViewChanged;
        }

        void scrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var atBottom = _scrollViewer.HorizontalOffset >= (_scrollViewer.ExtentWidth - _scrollViewer.ViewportWidth) - 200;
            if (atBottom)
            {
                ViewModel.LoadData();
            }
            if(_scrollViewer.HorizontalOffset > 120) PyControl.Visibility = Visibility.Collapsed;
            else
            {
                PyControl.Visibility = Visibility.Visible;
            }
        }

        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
         
        }

        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                ViewModel.UpdateChangedItem();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (TopAppBar != null && TopAppBar.IsOpen)
            {
                TopAppBar.IsOpen = false;
            }
            _navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void pageRoot_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CalculateHeight();
        }
        private void CalculateHeight()
        {
            var bounds = Window.Current.Bounds;
            ViewModel.CalculateWidthHeight(bounds.Height, bounds.Width);
        }
     
    }
}

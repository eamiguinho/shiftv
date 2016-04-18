using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Shiftv.Common;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Shiftv.DataModel;
using Shiftv.ViewModels.Shows.Pages;


namespace Shiftv.Views.Shows.Pages
{

    public sealed partial class DashboardPage : Page
    {
        private NavigationHelper navigationHelper;

        public DashboardViewModel ViewModel { get { return (DashboardViewModel)DataContext; } }
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public DashboardPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            CalculateMarginTop();
        }

        private void CalculateMarginTop()
        {
            var bounds = Window.Current.Bounds;
            ViewModel.MarginTopHeight = new Thickness(0, bounds.Height - 440, 0, 0);
        }
   
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Assign a collection of bindable groups to this.DefaultViewModel["Groups"]
        }

        #region NavigationHelper registration


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void Play_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var senderConverted = (Grid) sender;
            ViewModel.EpisodeFromListClick(senderConverted.DataContext as EpisodeDataModel);
        }

        private void PlayButton_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var senderConv = (Grid) sender;
            var path = (Path)senderConv.FindName("ElipseIcon");
            if (path != null) path.Fill = new SolidColorBrush(Color.FromArgb(255, 41, 162, 159));
            var icon = (Path)senderConv.FindName("PlayIcon");
            if (icon != null) icon.Fill = new SolidColorBrush(Color.FromArgb(255, 41, 162, 159));
        }

        private void PlayButton_OnPointerOut(object sender, PointerRoutedEventArgs e)
        {
            var senderConv = (Grid)sender;
            var path = (Path)senderConv.FindName("ElipseIcon");
            if (path != null) path.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            var icon = (Path)senderConv.FindName("PlayIcon");
            if (icon != null) icon.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        }

        private void SeenButton_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var senderConverted = (Grid)sender;
            ViewModel.SeenTapped(senderConverted.DataContext as EpisodeDataModel);
        }

        private void SeenButton_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var senderConv = (Grid)sender;
            var path = (Path)senderConv.FindName("SeenElipseIcon");
            if (path != null) path.Fill = new SolidColorBrush(Color.FromArgb(255, 41, 162, 159));
            var icon = (FontIcon)senderConv.FindName("SeenIcon");
            if (icon != null) icon.Foreground = new SolidColorBrush(Color.FromArgb(255, 41, 162, 159));
        }

        private void SeenButton_OnPointerOut(object sender, PointerRoutedEventArgs e)
        {
            var senderConv = (Grid)sender;
            var path = (Path)senderConv.FindName("SeenElipseIcon");
            if (path != null) path.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            var icon = (FontIcon)senderConv.FindName("SeenIcon");
            if (icon != null) icon.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        }

        private void SeenTopButton_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var senderConv = (Grid)sender;
            var path = (Path)senderConv.FindName("SeenTopElipseIcon");
            if (path != null) path.Fill = new SolidColorBrush(Color.FromArgb(255, 41, 162, 159));
            var path2 = (Path)senderConv.FindName("SeenTopArrowUpIcon");
            if (path2 != null) path2.Fill = new SolidColorBrush(Color.FromArgb(255, 41, 162, 159));
            var icon = (FontIcon)senderConv.FindName("SeenIconTop");
            if (icon != null) icon.Foreground = new SolidColorBrush(Color.FromArgb(255, 41, 162, 159));
        }

        private void SeenTopButton_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            var senderConv = (Grid)sender;
            var path = (Path)senderConv.FindName("SeenTopElipseIcon");
            if (path != null) path.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            var path2 = (Path)senderConv.FindName("SeenTopArrowUpIcon");
            if (path2 != null) path2.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            var icon = (FontIcon)senderConv.FindName("SeenIconTop");
            if (icon != null) icon.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        }

        private void SeenTopButton_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var senderConverted = (Grid)sender;
            ViewModel.SeenTopTapped(senderConverted.DataContext as EpisodeDataModel);
        }
    }
}

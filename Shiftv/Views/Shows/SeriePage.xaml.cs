using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AdRotator;
using AdRotator.Model;
using Callisto.Controls;
using Shiftv.Common;
using Shiftv.DataModel;
using Shiftv.ViewModels.Shows;

namespace Shiftv.Views.Shows
{
    public sealed partial class SeriePage
    {
        private readonly NavigationHelper _navigationHelper;

        public SerieViewModel ViewModel { get { return (SerieViewModel)DataContext; } }


        public NavigationHelper NavigationHelper
        {
            get { return _navigationHelper; }
        }

        public List<string> Sections;

        public SeriePage()
        {
            InitializeComponent();
            _navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += navigationHelper_LoadState;
            CalculateMarginTop();
            Sections = new List<string>();
        }

        private void CalculateMarginTop()
        {
            var bounds = Window.Current.Bounds;
            ViewModel.MarginTopHeight = new Thickness(0, bounds.Height - 440, 0, 0);
        }

    
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        #region NavigationHelper registration



        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedTo(e);
            var pageType = App.RootFrame.BackStack[App.RootFrame.BackStack.Count - 1];
            if (pageType.SourcePageType == typeof(SeriePage))
            {
                App.RootFrame.BackStack.RemoveAt(App.RootFrame.BackStack.Count - 1);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedFrom(e);
        }

        #endregion


        private void Hub_LayoutUpdated(object sender, object e)
        {

        }



        private void UIElement_OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var stackPanel = (StackPanel)sender;
            stackPanel.Opacity = 0;
        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            foreach (HubSection section in ShowHub.Sections)
            {
                if (section.Header != null)
                {
                    Sections.Add(section.Header.ToString());
                }
            }
            cvs4.Source = Sections;
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var stackPanel = (StackPanel)sender;
            var parent = stackPanel.Parent;
            var comment = ((Grid)parent).DataContext as CommentsDataModel;
            ViewModel.CommentClick(comment);
        }

        private void MyAdRotator_OnLoaded(object sender, RoutedEventArgs e)
        {
            var s = (AdRotatorControl)sender;
            s.PlatformAdProviderComponents.Add(AdType.PubCenter, typeof(Microsoft.Advertising.WinRT.UI.AdControl));
        }

        private void ClickOpenAppBar(object sender, RoutedEventArgs e)
        {
            BottomAppBar.IsOpen = true;
        }

        private void TappedMainGrid(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.IsRatingVisible = false;
            ViewModel.IsAddHistoryVisible = false;
        }

        private async void Rating_OnValueChanged(object sender, ValueChangedEventArgs<double> e)
        {
            if (e.OldValue != e.NewValue && e.NewValue != ViewModel.Show.RatedValue)
            {
                await ViewModel.RatingValueChangedSubmit(e.NewValue);
            }
        }

        private void CommentTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
          
           
        }
    }
}

using System;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236
using GalaSoft.MvvmLight.Messaging;
using Shiftv.DataModel;
using Shiftv.ViewModels.AppBar;

namespace Shiftv.Views.AppBar
{
    public sealed partial class UserDisplayControl : UserControl
    {
        private ActivityState _activityState;
        private ActivityState _activityStateSearch;
        private UserDisplayControlViewModel _viewModel;
        public UserDisplayControlViewModel ViewModel { get { return (UserDisplayControlViewModel)DataContext; } }
        public UserDisplayControl()
        {
            this.InitializeComponent();
         //   socialGridContent.Height = Window.Current.Bounds.Height;
            this.Loaded +=UserDisplayControl_Loaded;
            _viewModel = (UserDisplayControlViewModel) DataContext;
            searchGridContent.Height = Window.Current.Bounds.Height;
            this.Loaded += SearchDisplayControl_Loaded;
            mainGrid.Width = Window.Current.Bounds.Width;
            mainGrid.Height = Window.Current.Bounds.Height;
            MainBorder.Width = Window.Current.Bounds.Width;
            MainBorder.Height = Window.Current.Bounds.Height;
            this.SizeChanged += UserDisplayControl_SizeChanged;
        }

        void UserDisplayControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            mainGrid.Width = Window.Current.Bounds.Width;
            mainGrid.Height = Window.Current.Bounds.Height;
            MainBorder.Width = Window.Current.Bounds.Width;
            MainBorder.Height = Window.Current.Bounds.Height;
        }

        private void SearchDisplayControl_Loaded(object sender, RoutedEventArgs e)
        {
            CloseSearch(false);
        }

        private void UserDisplayControl_Loaded(object sender, RoutedEventArgs e)
        {
            CloseActivities(false);
        }

        public enum ActivityState   
        {
            Open,
            Close   
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (_activityState == ActivityState.Close)
            {   
                OpenActivities(true);
            }
            else
            {
                CloseActivities(true);
            }
            
        }

        private async void CloseActivities(bool b)
        {
            VisualStateManager.GoToState(this, "Close", b);
            _activityState = ActivityState.Close;
            await Task.Delay(350);
            searchGrid.Visibility = Visibility.Visible;
            MainBorder.Visibility = Visibility.Collapsed;
        }

        private void OpenActivities(bool b)
        {
            searchGrid.Visibility = Visibility.Collapsed;
            VisualStateManager.GoToState(this, "Open", b);
            _activityState = ActivityState.Open;
            MainBorder.Visibility = Visibility.Visible;
        }

        private void Timeline_OnCompleted(object sender, object e)
        {
            if(_activityState == ActivityState.Open) _viewModel.LoadData();
        }

        private void UserTapped(object sender, TappedRoutedEventArgs e)
        {
            var stackPanel = (StackPanel)sender;
            var parent = stackPanel.Parent;
            var activity = ((Grid)parent).DataContext as ActivityDataModel;
            ViewModel.UserCliked(activity);
        }

        private void ActivityTapped(object sender, TappedRoutedEventArgs e)
        {
            var stackPanel = (Grid)sender;
            var parent = stackPanel.Parent;
            var activity = ((Grid)parent).DataContext as ActivityDataModel;
            ViewModel.ActivityClick(activity);
        }

        private void SearchOnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (_activityStateSearch == ActivityState.Close)
            {
                OpenSearch(true);
            }
            else
            {
                CloseSearch(true);
            }

        }



        private async void CloseSearch(bool b)
        {
            FocusGive.Focus(FocusState.Programmatic);
            VisualStateManager.GoToState(this, "CloseSearchState", b);
            _activityStateSearch = ActivityState.Close; 
           // socialGrid.Visibility = Visibility.Visible;
            await Task.Delay(500);
            MainBorder.Visibility = Visibility.Collapsed;
        }

        private void OpenSearch(bool b)
        {
          //  socialGrid.Visibility = Visibility.Collapsed;
            VisualStateManager.GoToState(this, "OpenSearchState", b);
            _activityStateSearch = ActivityState.Open;
            SearchBox.Focus(FocusState.Programmatic);
            MainBorder.Visibility = Visibility.Visible;
        }

        private void SearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
           // FocusGive.Focus(FocusState.Programmatic);
            FocusManager.TryMoveFocus(FocusNavigationDirection.Next);
        }

        private void SearchBox_OnKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != VirtualKey.Escape) return;
            SearchBox.QueryText = "";
            CloseSearch(true);
        }

        private void MainBorder_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (_activityState == ActivityState.Open)
            {
                CloseActivities(true);
            }
            if (_activityStateSearch == ActivityState.Open)
            {
                CloseSearch(true);
            }
        }
    }
}

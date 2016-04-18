using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236
using Shiftv.DataModel;
using Shiftv.ViewModels.AppBar;

namespace Shiftv.Views.AppBar
{
    public sealed partial class SearchDisplayControl : UserControl
    {
        private ActivityState _activityState;
        public SearchDisplayControlViewModel ViewModel { get { return (SearchDisplayControlViewModel)DataContext; } }
        public SearchDisplayControl()
        {
            this.InitializeComponent();
            searchGridContent.Height = Window.Current.Bounds.Height;
            this.Loaded +=SearchDisplayControl_Loaded;
        }

        private void SearchDisplayControl_Loaded(object sender, RoutedEventArgs e)
        {
            CloseSearch(false);
        }

        public enum ActivityState     
        {
            Open,
            Close   
        }

        private void SearchOnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (_activityState == ActivityState.Close)
            {   
                OpenSearch(true);
            }
            else
            {
                CloseSearch(true);
            }
            
        }

        private void CloseSearch(bool b)
        {
            VisualStateManager.GoToState(this, "CloseSearchState", b);
            _activityState = ActivityState.Close;;
        }
            
        private void OpenSearch(bool b)
        {
            VisualStateManager.GoToState(this, "OpenSearchState", b);
            _activityState = ActivityState.Open;
        }

        private void SearchUserTapped(object sender, TappedRoutedEventArgs e)
        {
            var stackPanel = (StackPanel)sender;
            var parent = stackPanel.Parent;
            var activity = ((Grid)parent).DataContext as ActivityDataModel;
            ViewModel.UserCliked(activity);
        }

        private void SearchActivityTapped(object sender, TappedRoutedEventArgs e)
        {
            var stackPanel = (StackPanel)sender;
            var parent = stackPanel.Parent;
            var activity = ((Grid)parent).DataContext as ActivityDataModel;
            ViewModel.ActivityClick(activity);
        }
    }
}

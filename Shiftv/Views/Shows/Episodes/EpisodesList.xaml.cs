using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Callisto.Controls;
using Shiftv.Common;
using Shiftv.DataModel;
using Shiftv.ViewModels.Shows.Episodes;

namespace Shiftv.Views.Shows.Episodes
{

    public sealed partial class EpisodesList
    {
        private readonly NavigationHelper _navigationHelper;
        private GridView _episodesGridView;
        public ListEpisodesViewModel ViewModel { get { return (ListEpisodesViewModel)DataContext; } }


        public NavigationHelper NavigationHelper
        {
            get { return _navigationHelper; }
        }

        public EpisodesList()
        {
            InitializeComponent();
            _navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += navigationHelper_LoadState;
            CalculateMarginTop();
        }

        private void CalculateMarginTop()
        {
            var bounds = Window.Current.Bounds;
            ViewModel.MarginTopHeight = new Thickness(0, bounds.Height - 425, 0, 0);
        }


    
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        #region NavigationHelper registration

      

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedTo(e);
            if(e.Parameter != null) ViewModel.LoadData((int)e.Parameter);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void Hub_LayoutUpdated(object sender, object e)
        {

        }

        private void ItemGridView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var myGridView = sender as GridView;
            if (myGridView == null) return;
            ViewModel.SelectedEpisodes.Clear();
            if(myGridView.Items == null) return;
            foreach (var x in myGridView.Items)
            {
                var xparsed = (EpisodeDataModel)x;
                xparsed.IsSelected = false;
            }
            foreach (var x in myGridView.SelectedItems)
            {
                var xparsed = (EpisodeDataModel) x;
                ViewModel.SelectedEpisodes.Add(xparsed);
                xparsed.IsSelected = true;
            } 
      
            ViewModel.IsAppBarOpen = ViewModel.SelectedEpisodes.Count > 0;
            ViewModel.UpdatePermissions();

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (_episodesGridView == null || _episodesGridView.Items == null) return;
            foreach (var x in _episodesGridView.Items)
            {
                var xparsed = (EpisodeDataModel)x;
                xparsed.IsSelected = true;
                _episodesGridView.SelectedItems.Add(xparsed);
                ViewModel.SelectedEpisodes.Add(xparsed);
            }
            ViewModel.UpdatePermissions();
        }

        private void ItemGridView_OnLoaded(object sender, RoutedEventArgs e)
        {
            _episodesGridView = (GridView) sender;
        }

        private void ButtonBase2_OnClick(object sender, RoutedEventArgs e)
        {
            if (_episodesGridView == null || _episodesGridView.Items == null) return;
            foreach (var x in _episodesGridView.Items)
            {
                var xparsed = (EpisodeDataModel)x;
                xparsed.IsSelected = false;
                _episodesGridView.SelectedItems.Remove(xparsed);
                ViewModel.SelectedEpisodes.Remove(xparsed);
            }
            ViewModel.UpdatePermissions();
        }

        private void ItemGridView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var episode = e.ClickedItem as EpisodeDataModel;
            ViewModel.OpenEpisode(episode);
        }

        private void ClickOpenAppBar(object sender, RoutedEventArgs e)
        {
            BottomAppBar.IsOpen = true;
        }

        private void TappedMainGrid(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.IsRatingVisible = false;
        }

        private async void Rating_OnValueChanged(object sender, ValueChangedEventArgs<double> e)
        {
            if (e.OldValue != e.NewValue && e.NewValue != ViewModel.Show.RatedValue)
            {
                await ViewModel.RatingValueChangedSubmit(e.NewValue);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Callisto.Controls;
using Microsoft.PlayerFramework;
using Shiftv.Common;
using Shiftv.ViewModels.Movies;
using Shiftv.Views.Movies.Player;

namespace Shiftv.Views.Movies
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class MoviePage2
    {
        private readonly NavigationHelper _navigationHelper;

        public MovieViewModel ViewModel { get { return (MovieViewModel)DataContext; } }
        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return _navigationHelper; }
        }

        public List<string> Sections;
        private MediaPlayer _player;
        private Canvas playerCanvas;
        private Grid playerGrid;
        private bool _alreadyLoaded;
        private TimeSpan _time;

        public MoviePage2()
        {
            InitializeComponent();
            _navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += navigationHelper_LoadState;
            CalculateMarginTop();
            Sections = new List<string>();

        }


        private void CalculateMarginTop()
        {
            //var bounds = Window.Current.Bounds;
            //ViewModel.MarginTopHeight = new Thickness(0, bounds.Height - 440, 0, 0);
        }




        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Assign a collection of bindable groups to this.DefaultViewModel["Groups"]
        }

        #region NavigationHelper registration


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedTo(e);
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


        private void TrailerPlayer_OnLoaded(object sender, RoutedEventArgs e)
        {
            if(!_alreadyLoaded)
            InitSmallPlayer(sender as MediaPlayer);
            else
            {
                _player.PlayResume(); 
               
            }
            //InitFullSCreenPlayer();
        }

        private async void InitSmallPlayer(MediaPlayer mediaPlayer)
        {
            var trailer = await ViewModel.GetTrailer();
            _player = mediaPlayer;
            _player.Source = trailer;
        }

  
   
        private void player_IsFullScreenChanged(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (e.NewValue && !e.OldValue)
            {
                //header.Visibility = Visibility.Collapsed;
                //details.Visibility = Visibility.Collapsed;
                //comments.Visibility = Visibility.Collapsed;
                //ViewModel.IsTopBarEnabled = true;
                _alreadyLoaded = true;
                _time = _player.Position;
                playerGrid.Children.Remove(playerCanvas);
                MainGrid.Children.Clear();
                MainGrid.Children.Add(playerCanvas);
                var offset = _player.TransformToVisual(PageRoot).TransformPoint(new Point());
                CanvasMover.X = -offset.X;
                CanvasMover.Y = -offset.Y;
                _player.Width = Window.Current.Bounds.Width;
                _player.Height = Window.Current.Bounds.Height;
              
                //scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                //scroll.HorizontalScrollMode = ScrollMode.Disabled;
                //Grid.SetRow(scroll, 0);
                //Grid.SetRowSpan(scroll, 2);
            }
            else
            {
                _time = _player.Position;
                _alreadyLoaded = true;
                MainGrid.Children.Clear();
                playerGrid.Children.Add(playerCanvas);
                MainGrid.Children.Add(Zoom);
                _player.Width = playerCanvas.Width;
                _player.Height = playerCanvas.Height;
                CanvasMover.X = 0;
                //ViewModel.IsTopBarEnabled = false;
                CanvasMover.Y = 0;
                //Grid.SetRow(scroll, 1);
                //Grid.SetRowSpan(scroll, 1);
                //scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                //scroll.HorizontalScrollMode = ScrollMode.Auto;
                //header.Visibility = Visibility.Visible;
                //details.Visibility = Visibility.Visible;
                //comments.Visibility = Visibility.Visible;
            }
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            App.RootFrame.Navigate(typeof (MoviePlayer));
        }

        private void playerCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
          //  throw new NotImplementedException();
        }

        private void PlayerCanvas_OnLoaded(object sender, RoutedEventArgs e)
        {
            playerCanvas = sender as Canvas;
            if(playerCanvas != null) CanvasMover = playerCanvas.RenderTransform as TranslateTransform;
        }

        public TranslateTransform CanvasMover { get; set; }

        private void PlayerGrid_OnLoaded(object sender, RoutedEventArgs e)
        {
            playerGrid = sender as Grid;
        }

        private void TrailerPlayer_OnMediaOpened(object sender, RoutedEventArgs e)
        {
            _player.SeekAsync(_time);
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
            if (e.OldValue != e.NewValue && e.NewValue != ViewModel.Movie.RatedValue)
            {
                await ViewModel.RatingValueChangedSubmit(e.NewValue);
            }
        }
    }
}

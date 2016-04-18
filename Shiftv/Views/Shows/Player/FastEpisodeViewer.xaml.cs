using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.PlayerFramework;
using Shiftv.Common;
using Shiftv.ViewModels.Shows.Episodes;
using Shiftv.ViewModels.Shows.Player;

namespace Shiftv.Views.Shows.Player
{

    public sealed partial class FastEpisodeViewer
    {

        private readonly NavigationHelper _navigationHelper;


        public NavigationHelper NavigationHelper
        {
            get { return _navigationHelper; }
        }

        public FastEpisodeViewerViewModel ViewModel { get { return (FastEpisodeViewerViewModel)DataContext; } }


        public FastEpisodeViewer()
        {
            InitializeComponent();
            _navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += navigationHelper_LoadState;
            _navigationHelper.SaveState += navigationHelper_SaveState;
            Player.KeyUp += player_KeyUp;
            Player.IsFullScreenChanged += player_IsFullScreenChanged;

        }

        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

     
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

      
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedTo(e);
            try
            {
                var data = (e.Parameter as string);
                ViewModel.EpisodeTitle = data;
                await LoadEpisode(data);
            }
            catch (Exception a)
            {
                Debug.WriteLine(a);
            }
        }

        private async Task LoadEpisode(string data)
        {
            StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(data.Replace(" ", "") + ".mp4");
            Player.Source = new Uri(file.Path);
            Player.Volume = 100;
            Player.MediaQuality = MediaQuality.HighDefinition;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedFrom(e);
        }


        private void playerCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!Player.IsFullScreen)
            {
                Player.Width = e.NewSize.Width;
                Player.Height = e.NewSize.Height;
            }
        }


        private void player_IsFullScreenChanged(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (e.NewValue && !e.OldValue)
            {
                Header.Visibility = Visibility.Collapsed;
                ViewModel.IsTopBarEnabled = true;
                var offset = Player.TransformToVisual(PageRoot).TransformPoint(new Point());
                CanvasMover.X = -offset.X;
                CanvasMover.Y = -offset.Y - Header.Height;
                Player.Width = Window.Current.Bounds.Width;
                Player.Height = Window.Current.Bounds.Height;
                Scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                Scroll.HorizontalScrollMode = ScrollMode.Disabled;
                Grid.SetRow(Scroll, 0);
                Grid.SetRowSpan(Scroll, 2);
            }
            else
            {
                Player.Width = PlayerCanvas.Width;
                Player.Height = PlayerCanvas.Height;
                CanvasMover.X = 0;
                ViewModel.IsTopBarEnabled = false;
                CanvasMover.Y = 0;
                Grid.SetRow(Scroll, 1);
                Grid.SetRowSpan(Scroll, 1);
                Scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                Scroll.HorizontalScrollMode = ScrollMode.Auto;
                Header.Visibility = Visibility.Visible;
            }
        }

        private void player_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Space)
            {
                if (Player.CurrentState == MediaElementState.Paused) Player.Play();
                if (Player.CurrentState == MediaElementState.Playing) Player.Pause();
            }
        }


        private void Player_OnTapped(object sender, DoubleTappedRoutedEventArgs doubleTappedRoutedEventArgs)
        {
            Player.IsFullScreen = !Player.IsFullScreen;
        }


        private void AppBarOpened(object sender, object e)
        {
            if (!ViewModel.IsTopBarEnabled)
            {
                PlayerTopAppBar.IsOpen = false;
            }
        }
    }
}

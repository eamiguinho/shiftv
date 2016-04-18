using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Media;
using Windows.Media.PlayTo;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Callisto.Controls;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.PlayerFramework;
using Newtonsoft.Json;
using Shiftv.Common;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Services;
using Shiftv.Core.Models.Shows;
using Shiftv.DataModel;
using Shiftv.ViewModels.Shows.Player;
using WinRTXamlToolkit.AwaitableUI;

namespace Shiftv.Views.Shows.Player
{

    public sealed partial class EpisodeViewer
    {

        private readonly NavigationHelper _navigationHelper;

        readonly SystemMediaTransportControls _systemControls;
        PlayToManager _playToManager;
        CoreDispatcher _dispatcher;
        private TimeSpan _savedCurrentTime;


        public NavigationHelper NavigationHelper
        {
            get { return _navigationHelper; }
        }

        public EpisodeViewerViewModel ViewModel { get { return (EpisodeViewerViewModel)DataContext; } }


        public 
            EpisodeViewer()
        {
            InitializeComponent();
            _navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += navigationHelper_LoadState;
            _navigationHelper.SaveState += navigationHelper_SaveState;
            Player.KeyUp += player_KeyUp;
            Player.MediaEnded += Player_MediaEnded;
            Player.IsFullScreenChanged += player_IsFullScreenChanged;
            _systemControls = SystemMediaTransportControls.GetForCurrentView();
            _systemControls.ButtonPressed += SystemControls_ButtonPressed;
            _systemControls.IsPlayEnabled = true;
            _systemControls.IsPauseEnabled = true;
            Scroll.IsHorizontalScrollChainingEnabled = false;
            Player.Focus(FocusState.Programmatic);

        }

        private void SystemControls_ButtonPressed(SystemMediaTransportControls sender, SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            switch (args.Button)
            {
                case SystemMediaTransportControlsButton.Play:
                    PlayMedia();
                    break;
                case SystemMediaTransportControlsButton.Pause:
                    PauseMedia();
                    break;
            }
        }
        private async void ReviewClicked(object sender, RoutedEventArgs e)
        {
            String pfn = Package.Current.Id.FamilyName;
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:REVIEW?PFN=" + pfn + ""));
        }

        private async void FacebookClick(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("http://facebook.com/shiftvapp"));
        }

        private async void TwitterClicked(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("http://twitter.com/shiftvapp"));
        }
        async void PlayMedia()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Player.Play());
        }

        async void PauseMedia()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Player.Pause());
        }
        void Player_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (Player.IsFullScreen)
            {
                ExitFullScreen();
            }
            //ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            //if (!localSettings.Values.ContainsKey("reviewPage"))
            //{
            //    Messenger.Default.Send("openReview");
            //}
            Player.Visibility = Visibility.Collapsed;
            PubGrid.Visibility = Visibility.Visible;
            RestartButton.Visibility = Visibility.Visible;
            PubGrid.Margin = new Thickness(0);
        }

        private void ExitFullScreen()
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
            Details.Visibility = Visibility.Visible;
           // Comments.Visibility = Visibility.Visible;
            NextButton.Visibility = Visibility.Visible;
            PreviousButton.Visibility = Visibility.Visible;
            StreamsButton.Visibility = Visibility.Visible;
            MyAdRotator.Visibility = Visibility.Visible;
            InternetConnectionError.Visibility = Visibility.Visible;
            AppBarButton.Visibility = Visibility.Visible;
            //BottomAppBar.IsSticky = true;
            //BottomAppBar.IsOpen = true;
            if (ViewModel.IsLoadingSubtitles) LoadingSubtitles.Visibility = Visibility.Visible;
            if (ViewModel.SubtitlesNotAvailable) NoSubs.Visibility = Visibility.Visible;
            if (ViewModel.NoSubtitlesLanguageOnSettings) NoSubsSettings.Visibility = Visibility.Visible;
        }

        private void player_IsFullScreenChanged(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (e.NewValue && !e.OldValue)
            {
                GoFullScreen();
            }
            else
            {
                ExitFullScreen();
            }
        }

        private void GoFullScreen()
        {
            //await Scroll.ScrollToHorizontalOffsetAsync(0);
            Header.Visibility = Visibility.Collapsed;
            Details.Visibility = Visibility.Collapsed;
           // Comments.Visibility = Visibility.Collapsed;
            NextButton.Visibility = Visibility.Collapsed;
            PreviousButton.Visibility = Visibility.Collapsed;
            MyAdRotator.Visibility = Visibility.Collapsed;
            InternetConnectionError.Visibility = Visibility.Collapsed;
            StreamsButton.Visibility = Visibility.Collapsed;
            AppBarButton.Visibility = Visibility.Collapsed;
            //BottomAppBar.IsSticky = false;
            //BottomAppBar.IsOpen = false;
            if (ViewModel.IsLoadingSubtitles) LoadingSubtitles.Visibility = Visibility.Collapsed;
            if (ViewModel.SubtitlesNotAvailable) NoSubs.Visibility = Visibility.Collapsed;
            if (ViewModel.NoSubtitlesLanguageOnSettings) NoSubsSettings.Visibility = Visibility.Collapsed;
            ViewModel.IsTopBarEnabled = true;
            var offset = Player.TransformToVisual(PageRoot).TransformPoint(new Point());
            CanvasMover.X = -offset.X - Scroll.HorizontalOffset;
            CanvasMover.Y = -offset.Y - Header.Height;
            Player.Width = Window.Current.Bounds.Width;
            Player.Height = Window.Current.Bounds.Height;
            Scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            Scroll.HorizontalScrollMode = ScrollMode.Disabled;
            Grid.SetRow(Scroll, 0);
            Grid.SetRowSpan(Scroll, 2);
        }

        private void player_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Space)
            {
                if (Player.CurrentState == MediaElementState.Paused) Player.Play();
                if (Player.CurrentState == MediaElementState.Playing) Player.Pause();
            }
            if (e.Key == VirtualKey.Left && Player.IsFullScreen)
            {
                Player.Position = Player.Position - new TimeSpan(0,0,0,30);
                e.Handled = true;
            }
            if (e.Key == VirtualKey.Right && Player.IsFullScreen)
            {
                Player.Position = Player.Position + new TimeSpan(0,0,0,30);
                e.Handled = true;
            }  
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
                var data = JsonConvert.DeserializeObject<EpisodeViewerDataModelMini>(e.Parameter as string);
                if (data.IsNextOrPrevious) App.RootFrame.BackStack.RemoveAt(App.RootFrame.BackStack.Count - 1);
                var show = CoreServices.Show.GetCurrentShow();
                var season = show != null ? show.Seasons.FirstOrDefault(x => x.Number == data.Season) : new Season();
                var data2 = new EpisodeViewerDataModel(new SeasonDataModel(season), data.SelectedEpisode);
                var episode = data2;
                ViewModel.LoadEpisodeData(episode);
                await LoadEpisode(data);
                _dispatcher = Window.Current.CoreWindow.Dispatcher;
                _playToManager = PlayToManager.GetForCurrentView();
                _playToManager.SourceRequested += playToManager_SourceRequested;
            }
            catch (Exception)
            {
                Debug.WriteLine(e);
            }
        }


        void playToManager_SourceRequested(PlayToManager sender, PlayToSourceRequestedEventArgs args)
        {
            var deferral = args.SourceRequest.GetDeferral();
            _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                args.SourceRequest.SetSource(Player.PlayToSource);
                deferral.Complete();
            });
        }
            
        private async Task LoadEpisode(EpisodeViewerDataModelMini data)
        {

            if (!data.IsLinkDownloaded)
            {
                var link = await ViewModel.GetLinks();
                if (link == null)
                {

                    return;
                }
                if (link.Links != null && link.Links.Count > 0) LoadCaptions();
                var linkStream = LinkAnalizer(link);
                if (linkStream == null)
                {

                    return;
                }
                var selectedLink =
                    ViewModel.AvailableStreams.FirstOrDefault(x => x.Model.StreamLink == linkStream.StreamLink);
                if (selectedLink != null) selectedLink.IsPlayingNow = true;
                Player.Source = new Uri(linkStream.StreamLink);
                Player.Volume = 100;
                Player.MediaQuality = MediaQuality.HighDefinition;
                PubGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(ViewModel.SelectedEpisode.Title.Replace(" ", "") + ".mp4");
                Player.Source = new Uri(file.Path);
                Player.Volume = 100;
                Player.MediaQuality = MediaQuality.HighDefinition;
            }
        }

        private ILinkInfo LinkAnalizer(IMediaStream link)
        {
            if (link.Links != null && link.Links.Count > 0)
            {
                var list = link.Links.Where(x => x.Quality != StreamQuality.ND).OrderBy(x => x.Velocity).ThenByDescending(x => x.FileSize).ToList();
                return list.First();
            }
            return null;
        }

        public async static Task<StorageFile> SaveAsync(
       Uri fileUri,
       StorageFolder folder,
       string fileName)
        {
            var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            var downloader = new BackgroundDownloader();
            var download = downloader.CreateDownload(
                fileUri,
                file);

            await download.StartAsync();

            return file;
        }

        private async void LoadCaptions()
        {
            var captions = await ViewModel.LoadCaptions();
            foreach (var caption in captions)
            {
                Player.AvailableCaptions.Add(caption);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _systemControls.PlaybackStatus = MediaPlaybackStatus.Closed;
            _navigationHelper.OnNavigatedFrom(e);
        }

        private void playerCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Player.IsFullScreen) return;
            Player.Width = e.NewSize.Width;
            Player.Height = e.NewSize.Height;
        }


        private void UIElement_OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var x = (StackPanel)sender;
            x.Opacity = 0;
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

        private void Player_OnCurrentStateChanged(object sender, RoutedEventArgs e)
        {
            switch (Player.CurrentState)
            {
                case MediaElementState.Playing:
                    ViewModel.AutoCheckinCheck();
                    _systemControls.PlaybackStatus = MediaPlaybackStatus.Playing;
                    break;
                case MediaElementState.Paused:
                    _systemControls.PlaybackStatus = MediaPlaybackStatus.Paused;
                    break;
                case MediaElementState.Stopped:
                    _systemControls.PlaybackStatus = MediaPlaybackStatus.Stopped;
                    break;
                case MediaElementState.Closed:
                    _systemControls.PlaybackStatus = MediaPlaybackStatus.Closed;
                    break;
            }
        }

        private void Player_OnMediaOpened(object sender, RoutedEventArgs e)
        {
            SystemMediaTransportControlsDisplayUpdater updater = _systemControls.DisplayUpdater;
            updater.Type = MediaPlaybackType.Video;
            // Music metadata.
            updater.VideoProperties.Title = ViewModel.SelectedEpisode.Title;
            updater.VideoProperties.Subtitle = ViewModel.SelectedEpisode.ShowName;

            // Set the album art thumbnail.
            // RandomAccessStreamReference is defined in Windows.Storage.Streams
            updater.Thumbnail =
               RandomAccessStreamReference.CreateFromUri(new Uri(ViewModel.SelectedEpisode.Screen));

            // Update the system media transport controls.
            updater.Update();
            Player.SeekAsync(_savedCurrentTime);
        }

        private void UserTapped(object sender, TappedRoutedEventArgs e)
        {
            var stackPanel = (StackPanel)sender;
            var parent = stackPanel.Parent;
            var comment = ((Grid)parent).DataContext as CommentsDataModel;
            ViewModel.CommentClick(comment);
        }

        private void pageRoot_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Player.IsFullScreen)
            {
                Player.Width = Window.Current.Bounds.Width;
                Player.Height = Window.Current.Bounds.Height;
            }
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var link = ((Grid)sender).DataContext as LinkInfoDataModel;
            if (link != null)
            {
                _savedCurrentTime = Player.Position;
                foreach (var linkInfoDataModel in ViewModel.AvailableStreams)
                {
                    linkInfoDataModel.IsPlayingNow = false;
                }
                Player.Visibility = Visibility.Visible;
                PubGrid.Visibility = Visibility.Collapsed;
                Player.Source = new Uri(link.Model.StreamLink);
                link.IsPlayingNow = true;
                ViewModel.IsStreamsVisible = false;
            }

        }

        private void TappedMainGrid(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.IsStreamsVisible = false;
            ViewModel.IsRatingVisible = false;
        }

        private void ClickOpenAppBar(object sender, RoutedEventArgs e)
        {
            BottomAppBar.IsOpen = true;
        }

        private async void Rating_OnValueChanged(object sender, ValueChangedEventArgs<double> e)
        {
            if (e.OldValue != e.NewValue && e.NewValue != ViewModel.SelectedEpisode.RatedValue)
            {
                await ViewModel.RatingValueChangedSubmit(e.NewValue);
            }
        }

        private void PlayAgain(object sender, RoutedEventArgs e)
        {
            Player.Visibility = Visibility.Visible;
            PubGrid.Visibility = Visibility.Collapsed;
            Player.Play();
        }
    }
}

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using BugSense;
using BugSense.Model;
using GalaSoft.MvvmLight.Messaging;
//using Microsoft.ApplicationInsights.Telemetry.WindowsStore;
using Newtonsoft.Json;
using Shiftv.Common;

using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Shiftv.Contracts.Data.Factories;
using Shiftv.Contracts.Data.Users;
using Shiftv.Contracts.Domain.Movies;
using Shiftv.Contracts.Domain.Shows;
using Shiftv.Contracts.Domain.Users;
using Shiftv.Contracts.Services;
using Shiftv.Global;
using Shiftv.Helpers;
using Shiftv.Views;
using Windows.UI.ApplicationSettings;
using Autofac;
using Microsoft.ApplicationInsights;
using Shiftv.Contracts.Services.Sync;
using Shiftv.ViewModels;
using Shiftv.Views.Settings;
using Xamarin;

namespace Shiftv
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton Application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            // BugSenseHandler.Instance.InitAndStartSession(new ExceptionManager(Current), "w8c9792d");
            //BugSenseHandler.Instance.InitAndStartSession(new ExceptionManager(Current), "w8c9792d");
            WindowsAppInitializer.InitializeAsync();
        }



        public static Frame RootFrame;
        private static bool _alreadyLoadDll;

        public static bool AlreadyLoadDll
        {
            get { return _alreadyLoadDll; }
            set { _alreadyLoadDll = value; }
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.Protocol)
            {
                ProtocolActivatedEventArgs eventArgs = args as ProtocolActivatedEventArgs;
                if (eventArgs != null)
                {
                    var authCodeArray = Regex.Split(eventArgs.Uri.Query, "code=");
                    var authCode = authCodeArray[1];
                    if (LoginVm != null)
                    {
                        await LoginVm.DoGetToken(authCode);
                    }
                }
                // TODO: Handle URI activation
                // The received URI is eventArgs.Uri.AbsoluteUri
            }
            else
            {
                base.OnActivated(args);
            }
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            SettingsPane.GetForCurrentView().CommandsRequested += SettingCharmManager_CommandsRequested;
#if DEBUG
            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            //InAppPurchases = CurrentApp.LicenseInformation;
            RootFrame = Window.Current.Content as Frame;
            AlreadyLoadDll = false;
            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active

            if (RootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                RootFrame = new Frame();
                //Associate the frame with a SuspensionManager key                                
                SuspensionManager.RegisterFrame(RootFrame, "AppFrame");
                // Set the default language
                //otFrame.Language = Windows.Globalization.
                //var myCulture = Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "pt-PT";
                //var myCulture2 = CultureInfo.CurrentCulture;
                RootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        //Something went wrong restoring state.
                        //Assume there is no state and continue
                    }
                }

                // Place the frame in the current Window
                SplashScreen splashScreen = e.SplashScreen;
                ExtendedSplash eSplash = new ExtendedSplash(splashScreen);
                // Register an event handler to be executed when the splash screen has been dismissed.
                Window.Current.Content = eSplash;
                Window.Current.Activate();

                await PerformDataFetch();
            }
            if (RootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                RootFrame.Navigate(typeof(LoginPage), e.Arguments);
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        public static LicenseInformation InAppPurchases
        {
            get { return CurrentApp.LicenseInformation; }
        }

        public static LoginViewModel LoginVm { get; set; }

        private void SettingCharmManager_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            args.Request.ApplicationCommands.Add(new SettingsCommand("logout", ShiftvHelpers.GetTranslation("SettingsLogInOut_Capital"), Logout));
            args.Request.ApplicationCommands.Add(new SettingsCommand(
               "Global Settings", ShiftvHelpers.GetTranslation("GlobalSettings_Capital"), (handler) => ShowGlobalSettingsFlyout()));
            args.Request.ApplicationCommands.Add(new SettingsCommand(
          "Facebook", "Facebook / Twitter", (handler) => TwitterFlyout()));
            // args.Request.ApplicationCommands.Add(new SettingsCommand(
            //"Tutorial", "Tutorial", (handler) => TutorialFlyout()));
            args.Request.ApplicationCommands.Add(new SettingsCommand(
               "DMCA", ShiftvHelpers.GetTranslation("DmcaAgg_Upper"), (handler) => ShowDMCAFlyout()));
            args.Request.ApplicationCommands.Add(new SettingsCommand(
               "Terms and Services", ShiftvHelpers.GetTranslation("TermsAndServices_Capital"), (handler) => ShowTermsFlyout()));
            args.Request.ApplicationCommands.Add(new SettingsCommand(
               "Legal Information", ShiftvHelpers.GetTranslation("LegalInformation_Capital"), (handler) => ShowLegalFlyout()));

        }

        private void TutorialFlyout()
        {
            RootFrame.Navigate(typeof(HelpPage));
        }

        private async void TwitterFlyout()
        {
            Windows.System.Launcher.LaunchUriAsync(new Uri("http://twitter.com/shiftvapp"));
            Windows.System.Launcher.LaunchUriAsync(new Uri("http://facebook.com/shiftvapp"));
        }



        private void ShowLegalFlyout()
        {
            var globalSettingsFlyout = new LegalInformation();
            globalSettingsFlyout.Show();
        }

        private void ShowTermsFlyout()
        {
            var globalSettingsFlyout = new TermsAndServices();
            globalSettingsFlyout.Show();
        }

        private void ShowDMCAFlyout()
        {
            var globalSettingsFlyout = new DMCA();
            globalSettingsFlyout.Show();
        }

        public void ShowGlobalSettingsFlyout()
        {
            var globalSettingsFlyout = new ShiftvSettings();
            globalSettingsFlyout.Show();
        }

        private async void Logout(IUICommand command)
        {
            if (CoreServices.User.GetCurrentUser() != null)
            {
                var messageDialog =
              new MessageDialog(
                  "Are you sure you want to logout? Remember to enter in Shiftv with different account you need to logout of trakt website in your browser as well");
                messageDialog.Commands.Add(new UICommand(
          ShiftvHelpers.GetTranslation("Yes"),
          uiCommand =>
          {
              DoLogout();
          }));
                messageDialog.Commands.Add(new UICommand(
                    ShiftvHelpers.GetTranslation("No")));
                await messageDialog.ShowAsync();
            }
            else DoLogout();
        }

        private static void DoLogout()
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values.Remove("Username");
            localSettings.Values.Remove("Password");
            localSettings.Values.Remove("firstTime");
            CoreServices.User.SetUser(null);
            CoreServices.Show.ClearTrending();
            CoreServices.Movie.ClearTrending();
            RootFrame.Navigate(typeof (LoginPage));
        }

        private async Task PerformDataFetch()
        {
            Random rnd = new Random();
            // data loading here
            var shows = await CoreServices.Show.GetTrending(true);

            var movies = await CoreServices.Movie.GetTrending(true);

            if (shows.IsOk && shows.Data != null && shows.Data.Count > 20)
            {
                await ImageHelper.GetShowImageAsync(new Uri(shows.Data[rnd.Next(15, 20)].Fanart.Full));
            }

            if (movies.IsOk && movies.Data != null && movies.Data.Count > 20)
            {
                await ImageHelper.GetMovieImageAsync(new Uri(movies.Data[rnd.Next(15, 20)].Fanart.Full));
            }

            ShowReview();
            ShowSoundClouder();

            RemoveExtendedSplash(movies, shows, rnd);
        }

        private async void ShowReview()
        {
            await Task.Delay(new TimeSpan(0, 15, 0));
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if (!localSettings.Values.ContainsKey("reviewPage"))
            {
                Messenger.Default.Send("openReview2");
            }
        }
        private async void ShowSoundClouder()
        {
            await Task.Delay(new TimeSpan(0, 0, 30));
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if (!localSettings.Values.ContainsKey("soundClouderPlayer"))
            {
                Messenger.Default.Send("soundClouderPlayer");
            }
        }

        private async void RemoveExtendedSplash(DataResult<List<IMiniMovie>> movies, DataResult<List<IMiniShow>> shows, Random rnd)
        {
            if (RootFrame != null)
            {
                var localSettings = ApplicationData.Current.LocalSettings;
                if (!string.IsNullOrEmpty(localSettings.Values["userData"] as string))
                {
                    var userDataString = localSettings.Values["userData"] as string;
                    var parsedUserData = JsonConvert.DeserializeObject<UserTokenDto>(userDataString);
                    if (parsedUserData.ExpiresAt > DateTime.Now)
                    {
                        CoreServices.User.SetUser(UserTokenDtoFactory.Create(parsedUserData));
                        if (!localSettings.Values.ContainsKey("firstTime"))
                        {
                            RootFrame.Navigate(typeof(HelpPage));
                        }
                        else
                        {
                            RootFrame.Navigate(typeof(ChooseSection));
                            Task.Run(async () =>
                            {
                                try
                                {
                                    var syncService = Ioc.Container.Resolve<ISyncService>();
                                    var uploadRatingsToTrakt = await syncService.UploadRatingsToTrakt();
                                    var uploadWatchedEpisodesToTrakt = await syncService.UploadWatchedEpisodesToTrakt();
                                    var uploadCommentsToTrakt = await syncService.UploadCommentsToTrakt();
                                    var syncWatchedShows = await syncService.SyncWatchedShows();
                                    var syncWatchedMovies = await syncService.SyncWatchedMovies();
                                    var syncShowRatings = await syncService.SyncShowRatings();
                                    //var syncSeasonsRatings = await syncService.SyncSeasonRatings();
                                    var syncEpisodesRatings = await syncService.SyncEpisodeRatings();
                                    var syncMovieRatings = await syncService.SyncMovieRatings();
                                  
                                }
                                catch (Exception e)
                                {

                                }
                            });
                            UpdateLiveTileWithData();
                        }
                        Window.Current.Content = RootFrame;
                        Window.Current.Activate();
                        return;
                    }
                }
                RootFrame.Navigate(typeof(LoginPage));
                Window.Current.Content = RootFrame;
                Window.Current.Activate();
                //var localSettings = ApplicationData.Current.LocalSettings;
                //if (!string.IsNullOrEmpty(localSettings.Values["Username"] as string) ||
                //    !string.IsNullOrEmpty(localSettings.Values["Password"] as string))
                //{
                //    var a =
                //        await
                //            CoreServices.User.GetUser(localSettings.Values["Username"] as string,
                //                localSettings.Values["Password"] as string);
                //    if (a.IsOk)
                //    {
                //        CoreServices.User.SetUser(a.Data);
                //        UpdateLiveTilesAsync();
                //    }
                //    if (!localSettings.Values.ContainsKey("firstTime"))
                //    {
                //        RootFrame.Navigate(typeof(HelpPage));
                //    }
                //    else
                //    {
                //        RootFrame.Navigate(typeof(ChooseSection));
                //    }

                //    Window.Current.Content = RootFrame;
                //    Window.Current.Activate();
                //}
                //else
                //{
                //    UpdateLiveTilesAsync(movies, shows, rnd);
                //    RootFrame.Navigate(typeof(LoginPage));
                //    Window.Current.Content = RootFrame;
                //    Window.Current.Activate();
                //}
            }
        }

        private async void UpdateLiveTilesAsync()
        {
            await CoreServices.Show.GetShowProgress();
        }

        public static async void UpdateLiveTileWithData(DataResult<List<IShowProgress>> dataResult = null)
        {
            if (DesignMode.DesignModeEnabled) return;
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);

            updater.Clear();
            if(dataResult == null)dataResult = await CoreServices.Show.GetShowProgress();
            if (dataResult.IsOk && dataResult.Data != null)
            {
                foreach (var showProgress in dataResult.Data.Where(x => x.EpisodesLeft.Any()).OrderBy(x => x.EpisodesLeft.Count()).Take(4)
                    )
                {
                    WideTile(showProgress, updater, TileTemplateType.TileWide310x150ImageAndText02);
                    WideTile(showProgress, updater, TileTemplateType.TileSquare310x310ImageAndText02);
                }
            }
        }

        private static void WideTile(IShowProgress showProgress, TileUpdater updater, TileTemplateType template)
        {
            try
            {
                if (DesignMode.DesignModeEnabled) return;
                var tile = TileUpdateManager.GetTemplateContent(template);
                XmlNodeList tileImageAttributes = tile.GetElementsByTagName("image");
                ((XmlElement)tileImageAttributes[0]).SetAttribute("src", showProgress.Show.Fanart.Thumb);
                ((XmlElement)tileImageAttributes[0]).SetAttribute("alt", "shiftv");
                var title = showProgress.Show.Title;
                title = Regex.Replace(title, @" ?\(.*?\)", string.Empty);
                tile.GetElementsByTagName("text")[0].InnerText = title;
                if (showProgress.EpisodesLeft.Count() == 1)
                    tile.GetElementsByTagName("text")[1].InnerText = string.Format("{0}", ShiftvHelpers.GetTranslation("NewEpisode"));
                else
                    tile.GetElementsByTagName("text")[1].InnerText = string.Format("{0} {1}",
                        showProgress.EpisodesLeft.Count(), ShiftvHelpers.GetTranslation("EpisodesLeftToSee"));
                var tileNotification = new Windows.UI.Notifications.TileNotification(tile);
                Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
            }
            catch (Exception e)
            {
               
            }
        
        }

        private static void UpdateLiveTilesAsync(DataResult<List<IMiniMovie>> movies, DataResult<List<IMiniShow>> shows, Random rnd)
        {
            if (DesignMode.DesignModeEnabled) return;
            if (movies.IsOk && movies.Data != null && movies.Data.Count > 20 && shows.IsOk && shows.Data != null &&
                shows.Data.Count > 20)
            {
                var updater = TileUpdateManager.CreateTileUpdaterForApplication();
                updater.EnableNotificationQueue(true);

                updater.Clear();

                for (int i = 1; i < 6; i++)
                {
                    var isPair = i % 2 == 0;
                    var data = isPair
                        ? movies.Data[rnd.Next(1, 20)].Fanart.Medium
                        : shows.Data[rnd.Next(1, 20)].Fanart.Medium;
                    var tile = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150Image);
                    XmlNodeList tileImageAttributes = tile.GetElementsByTagName("image");
                    ((XmlElement)tileImageAttributes[0]).SetAttribute("src", data);
                    ((XmlElement)tileImageAttributes[0]).SetAttribute("alt", "shiftv");
                    updater.Update(new TileNotification(tile));
                }
            }
        }

        internal async void RemoveExtendedSplash()
        {

        }

        //private async void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        //{
        //    string[] linkInfo = e.UserState as string[];
        //    filetitle = linkInfo[1];
        //    filesave = (filetitle);
        //    var isolatedfile = IsolatedStorageFile.GetUserStoreForApplication();
        //    using (IsolatedStorageFileStream stream = isolatedfile.OpenFile(filesave, System.IO.FileMode.Create))
        //    {
        //        byte[] buffer = new byte[e.Result.Length];
        //        while (e.Result.Read(buffer, 0, buffer.Length) > 0)
        //        {
        //            stream.Write(buffer, 0, buffer.Length);
        //        }
        //    }
        //    try
        //    {
        //        StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
        //        StorageFile torrentfile = await local.GetFileAsync(filesave);
        //        Windows.System.Launcher.LaunchFileAsync(torrentfile);
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("File Not Found");
        //    }
        //}

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }


    }
}

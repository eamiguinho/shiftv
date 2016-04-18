using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using Shiftv.Common;
using Shiftv.Contracts.Services;
using Shiftv.ViewModels;

namespace Shiftv.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
            LoadSlider();
            _navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += navigationHelper_LoadState;
            //WebView.NavigationStarting += WebView_NavigationStarting;
        }

        async void WebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            var x = args.Uri;
            if (args.Uri.Host == "shiftvapi.azurewebsites.net")
            {
                var authCodeArray = Regex.Split(args.Uri.Query, "code=");
                var authCode = authCodeArray[1];
                await ViewModel.DoGetToken(authCode);
            }
        }

        public DispatcherTimer Timer = new DispatcherTimer();
        public List<string> Images = new List<string>();
        private int _currentImageCount = 1;
        private NavigationHelper _navigationHelper;
        public NavigationHelper NavigationHelper
        {
            get { return _navigationHelper; }
        }

        public LoginViewModel ViewModel { get { return (LoginViewModel)DataContext; } }

        public void LoadSlider()
        {
            Timer.Tick += dispatcherTimer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 10);
            Timer.Start();
        }

        #region NavigationHelper registration

        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Assign a collection of bindable groups to this.DefaultViewModel["Groups"]
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (Timer != null && Timer.IsEnabled) Timer.Stop();
            _navigationHelper.OnNavigatedFrom(e);
        }
        #endregion
        private void dispatcherTimer_Tick(object sender, object e)
        {
            if (_currentImageCount == 5) _currentImageCount = 0;
            switch (_currentImageCount)
            {
                case 0:
                    FadeOut5.Begin();
                    FadeIn1.Begin();
                    break;
                case 1:
                    FadeOut1.Begin();
                    FadeIn2.Begin();
                    break;
                case 2:
                    FadeOut2.Begin();
                    FadeIn3.Begin();
                    break;
                case 3:
                    FadeOut3.Begin();
                    FadeIn4.Begin();
                    break;
                case 4:
                    FadeOut4.Begin();
                    FadeIn5.Begin();
                    break;
            }
            _currentImageCount++;
        }

        private async void TextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var textBlock = sender as TextBlock;
            if (textBlock == null) return;
            if (CoreServices.User.GetCurrentUser() == null && string.IsNullOrEmpty(textBlock.Text)) return;
            LoginGridFadeOut.Completed += LoginGridFadeOutOnCompleted;

            LoginGridFadeOut.Begin();
        }

        private void LoginGridFadeOutOnCompleted(object sender, object o)
        {
            UsernameGridFadeIn.Completed += UsernameGridFadeInOnCompleted;
            UsernameGridFadeIn.Begin();
            var localSettings = ApplicationData.Current.LocalSettings;

            if (!localSettings.Values.ContainsKey("firstTime") && CoreServices.User.GetCurrentUser() != null )
            {
                SyncGridFadeIn.Begin();
            }

        }

        private async void UsernameGridFadeInOnCompleted(object sender, object o)
        {
            await ViewModel.SyncData();
            Timer.Stop();
            var localSettings = ApplicationData.Current.LocalSettings;
            if (!localSettings.Values.ContainsKey("firstTime"))
            {
                App.RootFrame.Navigate(typeof(HelpPage));
            }
            else
            {
                App.RootFrame.Navigate(typeof(ChooseSection));
            }
        }

     

        private void WhithoutLogin_Tapped(object sender, TappedRoutedEventArgs e)
        {
            LoginGridFadeOut.Completed += LoginGridFadeOutOnCompleted;
            LoginGridFadeOut.Begin();
        }

        private void UIElement_OnKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                ViewModel.LoginButtonPressedCommand();
               // FocusGive.Focus(FocusState.Programmatic);
            }
        }
    }
}

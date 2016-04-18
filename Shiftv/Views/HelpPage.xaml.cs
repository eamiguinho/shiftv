using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class HelpPage : Page
    {
        public HelpPage()
        {
            this.InitializeComponent();
            LoadSlider();
            _navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += navigationHelper_LoadState;
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



        private void CloseTapped(object sender, TappedRoutedEventArgs e)
        {
            App.RootFrame.Navigate(typeof(ChooseSection));
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using Shiftv.Contracts.Services;
using Shiftv.ViewModels;
using Shiftv.Views.Movies;
using Shiftv.Views.Movies.Pages;
using Shiftv.Views.Shows.Pages;

namespace Shiftv.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChooseSection : Page
    {
        public ChooseSection()
        {
            this.InitializeComponent(); LoadSlider();
        }
        public DispatcherTimer Timer = new DispatcherTimer();
        public DispatcherTimer Timer2 = new DispatcherTimer();
        public List<string> Images = new List<string>();
        private int _currentImageCount = 1;
        private int _currentImageCount2 = 1;

        public async void LoadSlider()
        {
            Timer.Tick += dispatcherTimer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 7);
            Timer.Start();
            await Task.Delay(1000);
            Timer2.Tick += dispatcherTimer2_Tick;
            Timer2.Interval = new TimeSpan(0, 0, 7);
            Timer2.Start();
        }

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
        
        private void dispatcherTimer2_Tick(object sender, object e)
        {
            if (_currentImageCount2 == 5) _currentImageCount2 = 0;
            switch (_currentImageCount2)
            {
                case 0:
                    FadeOut10.Begin();
                    FadeIn6.Begin();
                    break;
                case 1:
                    FadeOut6.Begin();
                    FadeIn7.Begin();
                    break;
                case 2:
                    FadeOut7.Begin();
                    FadeIn8.Begin();
                    break;
                case 3:
                    FadeOut8.Begin();
                    FadeIn9.Begin();
                    break;
                case 4:
                    FadeOut9.Begin();
                    FadeIn10.Begin();
                    break;
            }
            _currentImageCount2++;
        }


        private void Shows_OnTapped(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            HoverShows.Visibility = Visibility.Visible;
        }

        private void Movies_OnTapped(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            HoverMovies.Visibility = Visibility.Visible;
        }

        private void Shows_OnExit(object sender, PointerRoutedEventArgs e)
        {
            HoverShows.Visibility = Visibility.Collapsed;
        }

        private void Movies_OnExit(object sender, PointerRoutedEventArgs e)
        {
            HoverMovies.Visibility = Visibility.Collapsed;
        }

        public ChooseSectionViewModel ViewModel { get { return (ChooseSectionViewModel) DataContext; } }

        private void TvShowsTapped(object sender, TappedRoutedEventArgs e)
        {
            Timer.Stop();
            Timer2.Stop();
            ViewModel.IsLoadingData = true;
            App.RootFrame.Navigate(typeof(TrendingShowsPage));
        }

        private void TvMoviesTapped(object sender, TappedRoutedEventArgs e)
        {
            Timer.Stop();
            Timer2.Stop();
            ViewModel.IsLoadingData = true;
            App.RootFrame.Navigate(typeof(TrendingMovies));
        }
    }
}
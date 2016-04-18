using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Messaging;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Shiftv.Views.AppBar
{
    public sealed partial class SoundClouderPub : UserControl
    {
        public SoundClouderPub()
        {
            this.InitializeComponent();
            this.Loaded += RemovePubControl_Loaded;
            this.Unloaded += RemovePubControl_UnLoaded;
            this.SizeChanged += RemovePubControl_SizeChanged;
            Window.Current.SizeChanged += Current_SizeChanged;
        }

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            MainGrid.Width = Window.Current.Bounds.Width;
            MainGrid.Height = Window.Current.Bounds.Height;
        }

        void RemovePubControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MainGrid.Width = Window.Current.Bounds.Width;
            MainGrid.Height = Window.Current.Bounds.Height;
        }

        private void RemovePubControl_UnLoaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister<string>(
                this, HandleStatusMessage);
        }

        private void RemovePubControl_Loaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Register<string>(
                this, HandleStatusMessage);
        }

        private void HandleStatusMessage(string obj)
        {
            if (obj == "soundClouderPlayer")
                MainGrid.Visibility = Visibility.Visible;
        }

   
        private void CloseTapped(object sender, TappedRoutedEventArgs e)
        {
            MainGrid.Visibility = Visibility.Collapsed;
        }
        private async void ReviewClicked(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://www.microsoft.com/en-us/store/apps/soundclouder/9wzdncrdpckb"));
        }
        private void DontAskAnymore(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["soundClouderPlayer"] = "No";
            MainGrid.Visibility = Visibility.Collapsed;
        }
    }
}

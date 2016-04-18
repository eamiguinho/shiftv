using System;
using System.Globalization;
using System.ServiceModel.Channels;
using Windows.ApplicationModel.Store;
using Windows.Globalization.DateTimeFormatting;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236
using AdRotator.Model;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Advertising.WinRT.UI;

namespace Shiftv.Views.AppBar
{
    public sealed partial class PubControl
    {
        public PubControl()
        {
            InitializeComponent();
            InitializePub();
            //somaAdViewer.Pub = 923883994;       // Developer pub ID for testing
            //somaAdViewer.Adspace = 65852437;   // Developer adSpace ID for testing

            //somaAdViewer.NewAdAvailable += somaAdViewer_NewAdAvailable;
            //somaAdViewer.AdError += somaAdViewer_AdError; 
            //somaAdViewer.StartAds();
        }



        private void somaAdViewer_NewAdAvailable(object sender, EventArgs e)
        {
           
        }

        private void InitializePub()
        {
            try
            {
                //AdRotatorSquare.PlatformAdProviderComponents.Add(AdType.PubCenter, typeof(AdControl));

                //AdRotatorSquare.PlatformAdProviderComponents.Add(AdType.AdDuplex, typeof(AdControl)); //<- Resolve to AdDuplex AdControl
                //AdRotatorSquare.PlatformAdProviderComponents.Add(AdType.Smaato, typeof(AdControl)); //<- Resolve to AdDuplex AdControl
            }
            catch
            {
                //e
            }
        }

        private async void RemoveAddsTapped(object sender, TappedRoutedEventArgs e)
        {
            Messenger.Default.Send("openBuy");
            //var inAppPurchase = App.InAppPurchases;
            //if (!inAppPurchase.ProductLicenses["NoAds"].IsActive)
            //{
            //    try
            //    {
            //        // The customer doesn't own this feature, so 
            //        // show the purchase dialog.

            //        var req = await CurrentApp.RequestProductPurchaseAsync("NoAds", false);

            //        var check = inAppPurchase.ProductLicenses["NoAds"].IsActive;
            //        //Check the license state to determine if the in-app purchase was successful.
            //    }
            //    catch (Exception)
            //    {
            //        // The in-app purchase was not completed because 
            //        // an error occurred.
            //    }
            //}
            //else
            //{
            //    // The customer already owns this feature.
            //}
        }

        private void AdControl_ErrorOccurred(object sender, AdErrorEventArgs e)
        {
            pubCenter.Visibility = Visibility.Collapsed;
            addDuplex.Visibility = Visibility.Visible;
        }

        private void PubCenter_OnAdRefreshed(object sender, RoutedEventArgs e)
        {
            pubCenter.Visibility = Visibility.Visible;
            addDuplex.Visibility = Visibility.Collapsed;
        }
    }
}

using System;
using Windows.ApplicationModel.Store;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using AdRotator.Model;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Advertising.WinRT.UI;

namespace Shiftv.Views.AppBar
{
    public sealed partial class PubControlVertical
    {
        public PubControlVertical()
        {
            this.InitializeComponent();
            InitializePub();
            //somaAdViewer.Pub = 923883994;       // Developer pub ID for testing
            //somaAdViewer.Adspace = 65852437;   // Developer adSpace ID for testing

            //somaAdViewer.NewAdAvailable += somaAdViewer_NewAdAvailable;
            //somaAdViewer.AdError += somaAdViewer_AdError;
            //somaAdViewer.StartAds();
        }
        void somaAdViewer_AdError(object sender, string ErrorCode, string ErrorDescription)
        {
          
        }


        private void somaAdViewer_NewAdAvailable(object sender, EventArgs e)
        {
         
        }
        private void InitializePub()
        {
            try
            {
                //VerticalAdRotator.PlatformAdProviderComponents.Add(AdType.PubCenter, typeof(AdControl));
                //VerticalAdRotator.PlatformAdProviderComponents.Add(AdType.AdDuplex, typeof(AdControl)); //<- Resolve to AdDuplex AdControl
                //VerticalAdRotator.PlatformAdProviderComponents.Add(AdType.Smaato, typeof(AdControl)); //<- Resolve to AdDuplex AdControl
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

            //        await CurrentApp.RequestProductPurchaseAsync("NoAds", false);
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
            addDuplex.Visibility = Visibility.Visible;
            pubCenter.Visibility = Visibility.Collapsed;
        }

        private void PubCenter_OnAdRefreshed(object sender, RoutedEventArgs e)
        {
            addDuplex.Visibility = Visibility.Collapsed;
            pubCenter.Visibility = Visibility.Visible;
        }
    }
}

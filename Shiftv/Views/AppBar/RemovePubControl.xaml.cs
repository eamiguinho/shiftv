using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236
using GalaSoft.MvvmLight.Messaging;
using Shiftv.Contracts.Services;
using Shiftv.Helpers;
using Shiftv.Views.Shows.Pages;

namespace Shiftv.Views.AppBar
{
    public sealed partial class RemovePubControl : UserControl
    {
        private bool _buyedSucess;

        public RemovePubControl()
        {
            this.InitializeComponent();
            MainGrid.Width = Window.Current.Bounds.Width;
            MainGrid.Height = Window.Current.Bounds.Height;
            this.Loaded += RemovePubControl_Loaded;
            this.Unloaded += RemovePubControl_UnLoaded;
            Window.Current.SizeChanged += Current_SizeChanged;
        }

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
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
            if (obj == "openBuy")
                MainGrid.Visibility = Visibility.Visible;
        }

        private void CloseTapped(object sender, TappedRoutedEventArgs e)
        {
            if (_buyedSucess == false)
            {
                MainGrid.Visibility = Visibility.Collapsed;
                BuyGrid.Visibility = Visibility.Visible;
                BuyGridExtra.Visibility = Visibility.Visible;
                ErrorGrid.Visibility = Visibility.Collapsed;
                CongratsGrid.Visibility = Visibility.Collapsed;
                GlobalBorder.Padding = new Thickness(30, 30, 30, 80);
                EmailError.Text = " ";
                EmailGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                var currentPage = App.RootFrame.CurrentSourcePageType;
                App.RootFrame.Navigate(currentPage);
            }
        }

        private async void RemoveAdsClick(object sender, RoutedEventArgs e)
        {
            LicenseInformation licenseInformation = CurrentApp.LicenseInformation;
            if (!licenseInformation.ProductLicenses["NoAds"].IsActive)
            {
                try
                {
                    // The customer doesn't own this feature, so 
                    // show the purchase dialog.

                    var req = await CurrentApp.RequestProductPurchaseAsync("NoAds", false);
                    if (licenseInformation.ProductLicenses["NoAds"].IsActive)
                    {
                        CongratsGrid.Visibility = Visibility.Visible;
                        BuyGrid.Visibility = Visibility.Collapsed;
                        BuyGridExtra.Visibility = Visibility.Collapsed;
                        GlobalBorder.Padding = new Thickness(30, 30, 30, 30);
                        _buyedSucess = true;
                    }
                    else
                    {
                        ShowErrorMessage();
                    }
                    //switch (req.Status)
                    //{
                    //    case ProductPurchaseStatus.Succeeded:
                    //        CongratsMessage.Text = "thanks! now you will not see more ads on shiftv";
                    //        CongratsGrid.Visibility = Visibility.Visible;
                    //        BuyGrid.Visibility = Visibility.Collapsed;
                    //        BuyGridExtra.Visibility = Visibility.Collapsed;
                    //        break;
                    //    case ProductPurchaseStatus.AlreadyPurchased:
                    //    case ProductPurchaseStatus.NotFulfilled:
                    //    case ProductPurchaseStatus.NotPurchased:
                    //        ErrorMessage.Text = "sorry :( something went wrong on the purchase please try again later";
                    //        ErrorGrid.Visibility = Visibility.Visible;
                    //        BuyGrid.Visibility = Visibility.Collapsed;
                    //        BuyGridExtra.Visibility = Visibility.Collapsed;
                    //        break;
                    //    default:
                    //        throw new ArgumentOutOfRangeException();
                    //}
                    //var check = inAppPurchase.ProductLicenses["NoAds"].IsActive;
                    //Check the license state to determine if the in-app purchase was successful.
                }
                catch (Exception)
                {
                    ShowErrorMessage();
                }
            }
            else
            {
                AlreadyGotMessage();
            }
        }

        private async void RemoveAds2Click(object sender, RoutedEventArgs e)
        {
            var user = CoreServices.User.GetCurrentUser();
            LicenseInformation licenseInformation = CurrentApp.LicenseInformation;
            if (!licenseInformation.ProductLicenses["NoAds2"].IsActive)
            {
                try
                {
                    // The customer doesn't own this feature, so 
                    // show the purchase dialog.
                    var req = await CurrentApp.RequestProductPurchaseAsync("NoAds2", false);
                    if (licenseInformation.ProductLicenses["NoAds2"].IsActive)
                    {
                        CongratsGrid2.Visibility = Visibility.Visible;
                        BuyGrid.Visibility = Visibility.Collapsed;
                        BuyGridExtra.Visibility = Visibility.Collapsed;
                        UserImage.ImageSource = new BitmapImage { UriSource = new Uri(user.UserSettings.User.Images.Avatar.Full) };
                        GlobalBorder.Padding = new Thickness(30, 30, 30, 30);
                        _buyedSucess = true;
                        CoreServices.User.SetUserAsSilverBadge();
                    }
                    else
                    {
                        ShowErrorMessage();
                    }
                    //switch (req.Status)
                    //{
                    //    case ProductPurchaseStatus.Succeeded:
                    //        CongratsMessage.Text = "thanks! now you will not see more ads on shiftv";
                    //        CongratsGrid.Visibility = Visibility.Visible;
                    //        BuyGrid.Visibility = Visibility.Collapsed;
                    //        BuyGridExtra.Visibility = Visibility.Collapsed;
                    //        break;
                    //    case ProductPurchaseStatus.AlreadyPurchased:
                    //    case ProductPurchaseStatus.NotFulfilled:
                    //    case ProductPurchaseStatus.NotPurchased:
                    //        ErrorMessage.Text = "sorry :( something went wrong on the purchase please try again later";
                    //        ErrorGrid.Visibility = Visibility.Visible;
                    //        BuyGrid.Visibility = Visibility.Collapsed;
                    //        BuyGridExtra.Visibility = Visibility.Collapsed;
                    //        break;
                    //    default:
                    //        throw new ArgumentOutOfRangeException();
                    //}
                    //var check = inAppPurchase.ProductLicenses["NoAds"].IsActive;
                    //Check the license state to determine if the in-app purchase was successful.
                }
                catch (Exception)
                {
                    ShowErrorMessage();
                }
            }
            else
            {
                AlreadyGotMessage();
            }
        }

        private void AlreadyGotMessage()
        {
            AlreadyGotGrid.Visibility = Visibility.Visible;
            BuyGrid.Visibility = Visibility.Collapsed;
            BuyGridExtra.Visibility = Visibility.Collapsed;
            EmailGrid.Visibility = Visibility.Collapsed;
            GlobalBorder.Padding = new Thickness(30, 30, 30, 30);
        }

        private void ShowErrorMessage()
        {
            ErrorGrid.Visibility = Visibility.Visible;
            BuyGrid.Visibility = Visibility.Collapsed;
            BuyGridExtra.Visibility = Visibility.Collapsed;
            EmailGrid.Visibility = Visibility.Collapsed;
            GlobalBorder.Padding = new Thickness(30, 30, 30, 30);
        }

        private void RemoveAds3Click(object sender, RoutedEventArgs e)
        {
            EmailGrid.Visibility = Visibility.Visible;
            BuyGrid.Visibility = Visibility.Collapsed;
            BuyGridExtra.Visibility = Visibility.Collapsed;
            GlobalBorder.Padding = new Thickness(30, 30, 30, 30);
        }

        public bool IsValid(string emailaddress)
        {
            Match match = Regex.Match(emailaddress, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            return match.Success;
        }

        private async void RemoveAds3Finish(object sender, RoutedEventArgs e)
        {
            EmailError.Text = " ";
            if (!IsValid(Email.Text))
            {
                EmailError.Text = ShiftvHelpers.GetTranslation("BuyPack3EmailError");
                return;
            }
            var user = CoreServices.User.GetCurrentUser();
            LicenseInformation licenseInformation = CurrentApp.LicenseInformation;
            if (!licenseInformation.ProductLicenses["NoAds3"].IsActive)
            {
                try
                {
                    // The customer doesn't own this feature, so 
                    // show the purchase dialog.
                    var req = await CurrentApp.RequestProductPurchaseAsync("NoAds3", false);
                    if (licenseInformation.ProductLicenses["NoAds3"].IsActive)
                    {
                        CongratsGrid3.Visibility = Visibility.Visible;
                        EmailGrid.Visibility = Visibility.Collapsed;
                        UserImage3.ImageSource = new BitmapImage { UriSource = new Uri(user.UserSettings.User.Images.Avatar.Full) };
                        _buyedSucess = true;
                        CoreServices.User.SetUserAsGoldBadge(Email.Text);
                    }
                    else
                    {
                        ShowErrorMessage();
                    }
                    //switch (req.Status)
                    //{
                    //    case ProductPurchaseStatus.Succeeded:
                    //        CongratsMessage.Text = "thanks! now you will not see more ads on shiftv";
                    //        CongratsGrid.Visibility = Visibility.Visible;
                    //        BuyGrid.Visibility = Visibility.Collapsed;
                    //        BuyGridExtra.Visibility = Visibility.Collapsed;
                    //        break;
                    //    case ProductPurchaseStatus.AlreadyPurchased:
                    //    case ProductPurchaseStatus.NotFulfilled:
                    //    case ProductPurchaseStatus.NotPurchased:
                    //        ErrorMessage.Text = "sorry :( something went wrong on the purchase please try again later";
                    //        ErrorGrid.Visibility = Visibility.Visible;
                    //        BuyGrid.Visibility = Visibility.Collapsed;
                    //        BuyGridExtra.Visibility = Visibility.Collapsed;
                    //        break;
                    //    default:
                    //        throw new ArgumentOutOfRangeException();
                    //}
                    //var check = inAppPurchase.ProductLicenses["NoAds"].IsActive;
                    //Check the license state to determine if the in-app purchase was successful.
                }
                catch (Exception)
                {
                    ShowErrorMessage();
                }
            }
            else
            {
                AlreadyGotMessage();
            }
        }
    }
}

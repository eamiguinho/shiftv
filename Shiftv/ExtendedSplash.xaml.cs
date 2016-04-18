using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using Shiftv.Common;
using Shiftv.Views;

namespace Shiftv
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExtendedSplash : Page
    {

        public ExtendedSplash(SplashScreen splash)
        {
            InitializeComponent();

            this.InitializeComponent();
            // Position the extended splash screen image in the same location as the splash screen image.
            this.extendedSplashImage.SetValue(Canvas.LeftProperty, splash.ImageLocation.X);
            this.extendedSplashImage.SetValue(Canvas.TopProperty, splash.ImageLocation.Y);
            this.extendedSplashImage.Height = splash.ImageLocation.Height;
            this.extendedSplashImage.Width = splash.ImageLocation.Width;

            // Position the extended splash screen's progress ring.
            this.splashProgressRing.SetValue(Canvas.TopProperty, splash.ImageLocation.Y + splash.ImageLocation.Height + 32);
            this.splashProgressRing.SetValue(Canvas.LeftProperty,
         splash.ImageLocation.X +
                 (splash.ImageLocation.Width / 2) - 15);
        }


    }
}

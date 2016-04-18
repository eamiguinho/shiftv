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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Shiftv.Views.AppBar
{
    public sealed partial class MainTopAppBar : UserControl
    {
        public MainTopAppBar()
        {
            this.InitializeComponent();
            if (Window.Current.Bounds.Width < 1600)
            {
                GridMovies.Visibility = Visibility.Collapsed;
                GridButtonMovies.Visibility = Visibility.Visible;
            }
            else
            {
                GridMovies.Visibility = Visibility.Visible;
                GridButtonMovies.Visibility = Visibility.Collapsed;
            }
        }
    }
}

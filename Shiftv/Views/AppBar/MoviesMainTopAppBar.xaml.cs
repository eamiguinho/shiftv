using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Shiftv.Views.AppBar
{
    public sealed partial class MoviesMainTopAppBar : UserControl
    {
        public MoviesMainTopAppBar()
        {
            this.InitializeComponent();
            if (Window.Current.Bounds.Width < 1600)
            {
                ButtonGoToShows.Visibility = Visibility.Visible;
                GridShows.Visibility = Visibility.Collapsed;
            }
            else
            {
                ButtonGoToShows.Visibility = Visibility.Collapsed;
                GridShows.Visibility = Visibility.Visible;
            }
        }
    }
}

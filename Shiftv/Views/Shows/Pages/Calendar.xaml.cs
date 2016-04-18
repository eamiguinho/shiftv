using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Shiftv.Common;
using Shiftv.DataModel;
using Shiftv.ViewModels.Shows.Pages;
using WinRTXamlToolkit.Controls.Extensions;

namespace Shiftv.Views.Shows.Pages
{

    public sealed partial class Calendar
    {
        private readonly NavigationHelper _navigationHelper;

        public NavigationHelper NavigationHelper
        {
            get
            {
                return _navigationHelper;
            }
        }

        public ScrollViewer ScrollViewer;

        public Calendar()
        {
            InitializeComponent();
            _navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += navigationHelper_LoadState;
            ItemGridView.Loaded += ItemGridViewOnLoaded;
        }

        private void ItemGridViewOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ScrollViewer = ItemGridView.GetFirstDescendantOfType<ScrollViewer>();
            ScrollViewer.ViewChanged += scrollViewer_ViewChanged;
        }

        void scrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var atBottom = ScrollViewer.HorizontalOffset >= (ScrollViewer.ExtentWidth - ScrollViewer.ViewportWidth) - 200;
            if (atBottom)
            {
                ViewModel.ReadMoreDays();
            }
        }

        public CalendarViewModel ViewModel { get { return (CalendarViewModel) DataContext; } }

        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Assign a collection of bindable groups to this.DefaultViewModel["Groups"]
        }

        #region NavigationHelper registration


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

       
    }
    public class MainGridTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EpisodeTemplate { get; set; }
        public DataTemplate NoEpisodeTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var calendarDataModel = item as CalendarDataModel;
            return calendarDataModel != null && calendarDataModel.Show == null ? NoEpisodeTemplate : EpisodeTemplate;
        }
    }
}

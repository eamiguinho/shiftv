using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Shiftv.Common
{
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value is bool && (bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is Visibility && (Visibility)value == Visibility.Visible;
        }
    }

    /// <summary>
    /// Value converter that translates false to <see cref="Visibility.Visible"/> and true to
    /// <see cref="Visibility.Collapsed"/>.
    /// </summary>
    public sealed class BooleanNotToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value is bool && (bool)value) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is Visibility && (Visibility)value == Visibility.Collapsed;
        }
    }   
    
    public sealed class NegateBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (!(value is bool) || !(bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is bool && (bool)value == false;
        }
    }

    public sealed class BooleanToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value is bool && (bool)value) ? 1 : 0.5;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return 1;
        }
    }

    public sealed class PercentageWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var percentage = (double)value;
            var width = Window.Current.Bounds.Width * percentage;
            return width;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var width = Window.Current.Bounds.Width * 100;
            return width;
        }
    }
}

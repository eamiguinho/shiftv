using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Shiftv.Controls
{
    public static class Tilt
    {
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool),
            typeof(Tilt), new PropertyMetadata(null, OnIsEnabledPropertyChanged));

        public static void SetIsEnabled(DependencyObject d, bool value)
        {
            d.SetValue(IsEnabledProperty, value);
        }

        public static bool GetIsEnabled(DependencyObject d)
        {
            return (bool)d.GetValue(IsEnabledProperty);
        }

        private static void OnIsEnabledPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var uiElement = d as UIElement;
            if (uiElement == null) return;
            uiElement.PointerPressed += uiElement_PointerPressed;
            uiElement.PointerReleased += uiElement_PointerReleased;
        }

        static void uiElement_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var uiElement = sender as UIElement;
            if (uiElement == null) return;
            var animation = new PointerUpThemeAnimation();
            Storyboard.SetTarget(animation, uiElement);
            var sb = new Storyboard();
            sb.Children.Add(animation);
            sb.Begin();
            uiElement.ReleasePointerCapture(e.Pointer);
        }

        static void uiElement_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var uiElement = sender as UIElement;
            if (uiElement == null) return;
            var animation = new PointerDownThemeAnimation();
            Storyboard.SetTarget(animation, uiElement);
            var sb = new Storyboard();
            sb.Children.Add(animation);
            sb.Begin();
            uiElement.CapturePointer(e.Pointer);
        }


    }
}

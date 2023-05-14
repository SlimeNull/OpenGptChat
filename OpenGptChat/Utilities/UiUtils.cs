using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OpenGptChat.Utilities
{
    public static class UiUtils
    {
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static CornerRadius GetCornerRadius(DependencyObject obj)
        {
            return (CornerRadius)obj.GetValue(CornerRadiusProperty);
        }

        public static void SetCornerRadius(DependencyObject obj, CornerRadius value)
        {
            obj.SetValue(CornerRadiusProperty, value);
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(UiUtils), new PropertyMetadata(new CornerRadius(), CornerRadiusChanged));

        private static void CornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FrameworkElement ele)
                return;

            if (GetBorderFromControl(ele) is not Border border)
                return;

            border.CornerRadius = (CornerRadius)e.NewValue;
        }

        private static Border? GetBorderFromControl(FrameworkElement control)
        {
            if (control is Border border)
                return border;

            int childrenCount = VisualTreeHelper.GetChildrenCount(control);
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(control, i);
                if (child is Border borderChild)
                    return borderChild;
            }

            return null;
        }
    }
}

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Energy.UI.Controls
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool valueAsBool = (bool) value;
            return valueAsBool ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueAsVisibility = (Visibility) value;
            return valueAsVisibility == Visibility.Visible;
        }
    }
}

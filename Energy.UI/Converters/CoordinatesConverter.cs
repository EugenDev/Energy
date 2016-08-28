using System;
using System.Globalization;
using System.Windows.Data;

namespace Energy.UI.Converters
{
    public class CoordinatesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
              object parameter, CultureInfo culture)
        {
            return (double)value + 75;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

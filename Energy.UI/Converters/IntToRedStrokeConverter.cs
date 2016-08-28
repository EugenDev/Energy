using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Energy.UI.Converters
{
    public class IntToRedStrokeConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var doubleValue = (double) value;

            var c = new Color
            {
                A = 255,
                G = 0,
                B = 0,
                R = (byte)(255 * doubleValue)
            };

            return new SolidColorBrush(c);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

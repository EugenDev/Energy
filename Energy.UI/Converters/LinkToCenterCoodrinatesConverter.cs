using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Energy.UI.Model;

namespace Energy.UI.Converters
{
    public class LinkToCenterCoodrinatesConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var link = (LinkModel) value;
            var centerX = link.From.X + (link.To.X - link.From.X) / 2.0;
            var centerY = link.From.Y + (link.To.Y - link.From.Y) / 2.0;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

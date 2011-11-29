using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace RoadTrafficConstructor.Converters
{
    public class BooleanToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var colors = parameter as Brush[];
            if (colors == null || colors.Length < 2) { return value; }

            if (!(value is bool)) { return value; }
            var booleanValue = (bool)value;

            return booleanValue ? colors[0] : colors[1];

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
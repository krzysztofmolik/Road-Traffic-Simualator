using System;
using System.Globalization;
using System.Windows.Data;

namespace RoadTrafficConstructor.Converters
{
    public class BoolNegation : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if ( !( value is bool ) ) { return value; }

            return !( ( bool ) value );
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new InvalidOperationException();
        }
    }
}
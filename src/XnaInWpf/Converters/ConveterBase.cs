using System;
using System.Globalization;
using System.Windows.Data;

namespace RoadTrafficConstructor.Converters
{
    public abstract class ConveterBase<TSource, TDestination> : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if ( value is TSource )
            {
                return this.Convert( ( TSource ) value );
            }

            throw new ArgumentException();
        }

        protected virtual TDestination Convert( TSource value )
        {
            throw new NotImplementedException();
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if ( value is TDestination )
            {
                return this.ConvertBack( ( TDestination ) value );
            }

            throw new ArgumentException();
        }

        protected virtual TSource ConvertBack( TDestination destination )
        {
            throw new NotImplementedException();
        }
    }
}
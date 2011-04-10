using System;
using System.Diagnostics.Contracts;

namespace RoadTrafficSimulator.Extension
{
    public static class LazyExtensions
    {
        public static T ValueOrThrow<T>( this Lazy<T> lazy )
        {
            Contract.Requires( lazy != null );
            if ( lazy.IsValueCreated == null )
            {
                throw new InvalidOperationException( "Lazy value is not created" );
            }

            return lazy.Value;
        }
    }
}
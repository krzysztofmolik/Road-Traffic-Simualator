using System;

namespace Common
{
    public static class ObjectExtension
    {
        public static TResult IfNotNull<T, TResult>( this T obj, Func<T, TResult> accesssor ) where T : class
        {
            if ( obj == null ) { return default( TResult ); }
            return accesssor( obj );
        }

        public static void IfNotNull<T>( this T obj, Action<T> accesssor ) where T : class
        {
            if ( obj == null ) { return; }
            accesssor( obj );
        }
    }
}
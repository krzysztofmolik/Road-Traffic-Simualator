using System;

namespace Common
{
    public static class ObjectExtensionAssert
    {
        public static T NotNull<T>(this T obj, string objectName ) where T : class
        {
            if ( obj == null )
            {
                throw new ArgumentNullException( objectName ?? string.Empty, "Object can't be null" );
            }

            return obj;
        }

        public static T NotNull<T>( this T obj ) where T : class
        {
            return obj.NotNull( "Unknow object name" );
        }
    }
}
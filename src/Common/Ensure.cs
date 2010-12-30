using System;
using System.Linq.Expressions;

namespace Common
{
    public static class Ensure
    {
        public static void NotNull<T>( T value, string name ) where T : class
        {
            if ( value == null )
            {
                throw new ArgumentNullException( name + " can't be null", name);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public static class ExtensionForIEnumerable
    {
        public static void ForEach<T>( this IEnumerable<T> list, Action<T> action )
        {
            if ( list == null )
            {
                throw new ArgumentNullException( "list" );
            }

            if ( action == null )
            {
                throw new ArgumentNullException( "action" );
            }

            foreach ( var item in list )
            {
                action( item );
            }
        }

        public static void ForEachUntil<T>( this IEnumerable<T> list, Action<T> action, Func<T,bool > predicate)
        {
            list.NotNull();
            action.NotNull();
            predicate.NotNull();

            foreach ( var item in list )
            {
                if( ! predicate( item ) )
                {
                    break;
                }

                action( item );
            }
        }

        public static bool IsEmpty<T>( this IEnumerable<T> collection )
        {
            return collection.Count() == 0;
        }

    }
}
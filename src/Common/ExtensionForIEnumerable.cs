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

        public static void ForEachUntil<T>( this IEnumerable<T> list, Action<T> action, Func<T, bool> predicate )
        {
            list.NotNull();
            action.NotNull();
            predicate.NotNull();

            foreach ( var item in list )
            {
                if ( !predicate( item ) )
                {
                    break;
                }

                action( item );
            }
        }

        public static bool IsEmpty<T>( this IEnumerable<T> collection )
        {
            return !collection.Any();
        }

        public static T OneBeforeLast<T>( this IEnumerable<T> collection )
        {
            // Base on implementation of Enumerable.Last()
            var list = collection as IList<T>;
            if ( list != null )
            {
                int count = list.Count;
                if ( count > 0 ) return list[ count - 2 ];
            }
            else
            {
                using ( var e = collection.GetEnumerator() )
                {
                    if ( !e.MoveNext() ) { throw new ArgumentException( "Collection contains less than 2 elements" ); }

                    if ( e.MoveNext() )
                    {
                        T result = default( T );
                        T oneBeforeLast;
                        do
                        {
                            oneBeforeLast = result;
                            result = e.Current;
                        } while ( e.MoveNext() );
                        return oneBeforeLast;
                    }
                }
            }
            return default( T );
        }

    }
}
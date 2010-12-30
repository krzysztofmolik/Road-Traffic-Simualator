using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace Common
{
    public static class EnumerableExtensionAssert
    {
        public static T NotEmpty<T>( this T collection ) where T : ICollection
        {
            if ( collection.Count == 0 )
            {
                throw new ArgumentException( "Collection can't be empty" );
            }

            return collection;
        }
    }

    public static class IListExtension
    {
        public static void MoveToBegin<T>( this IList<T> list, T value )
        {
            list.NotNull();
            Assert.That( list, Contains.Item( value ) );

            list.Remove( value );
            list.Insert( 0, value );
        }
    }
}
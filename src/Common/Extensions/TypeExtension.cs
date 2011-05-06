using System;
using System.Linq.Expressions;

namespace Common.Extensions
{
    public static class TypeExtension
    {
        public static Func<T> GetConstructor<T>( this Type type )
        {
            return Expression.Lambda<Func<T>>(Expression.New(type)).Compile();
        }
    }
}
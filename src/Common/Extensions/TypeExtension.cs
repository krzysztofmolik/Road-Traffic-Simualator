using System;
using System.Linq.Expressions;
using System.Linq;

namespace Common.Extensions
{
    public static class TypeExtension
    {
        public static Func<T> GetConstructor<T>( this Type type )
        {
            return Expression.Lambda<Func<T>>(Expression.New(type)).Compile();
        }

        public static bool IsImplementingInterface<TInterface>( this Type type ) where TInterface : class
        {
            return type.GetInterfaces().Any(s => s == typeof (TInterface));
        }
    }
}
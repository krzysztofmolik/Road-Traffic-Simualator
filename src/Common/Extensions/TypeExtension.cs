using System;
using System.Linq.Expressions;
using System.Linq;

namespace Common.Extensions
{
    public static class TypeExtension
    {
        public static Func<T> GetConstructor<T>( this Type type )
        {
            return Expression.Lambda<Func<T>>( Expression.New( type ) ).Compile();
        }

        public static Func<TArg1, T> GetConstrcutor<TArg1, T>( this Type type )
        {
            var arg1Paramter = Expression.Parameter( typeof( TArg1 ) );
            return Expression.Lambda<Func<TArg1, T>>( Expression.New( type ), arg1Paramter ).Compile();
        }

        public static Func<TArg1, TArg2, T> GetConstrcutor<TArg1, TArg2, T>( this Type type )
        {
            var arg1Paramter = Expression.Parameter( typeof( TArg1 ) );
            var arg2Paramter = Expression.Parameter( typeof( TArg2 ) );
            return Expression.Lambda<Func<TArg1, TArg2, T>>( Expression.New( type ), arg1Paramter, arg2Paramter ).Compile();
        }

        public static bool IsImplementingInterface<TInterface>( this Type type ) where TInterface : class
        {
            return type.GetInterfaces().Any( s => s == typeof( TInterface ) );
        }
    }
}
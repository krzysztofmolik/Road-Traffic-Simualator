using System;
using System.Linq.Expressions;

namespace Common.Extensions
{
    public static class ExpressionExtensions
    {
        public static TExpression As<TExpression>( this Expression expression ) where TExpression : Expression
        {
            if ( expression == null ) throw new ArgumentNullException( "expression" );
            var tExpression = expression as TExpression;
            if ( tExpression == null ) { throw new InvalidCastException(); }

            return tExpression;
        }
    }

    public static class MemberExpressionExtensions
    {
        public static TValue GetValue<TValue>( this MemberExpression memberExpression )
        {
            if( memberExpression == null ) throw new ArgumentNullException( "memberExpression" );

            return (TValue) Expression.Lambda( memberExpression.Expression ).Compile().DynamicInvoke();
        }
    }
}
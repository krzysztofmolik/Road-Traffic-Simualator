using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Common;
using RoadTrafficSimulator.Infrastructure.Controls;
using Common.Extensions;
using System.Linq;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands
{
    public static class Actions
    {
        public static UseCtorToCreateParamter<T> Ctor<T>( Expression<Func<T>> ctor )
        {
            var newExpression = ctor.Body as NewExpression;
            if ( newExpression == null ) { throw new ArgumentException(); }

            Is.Context.Clear();
            newExpression.Arguments.ForEach( exp => Expression.Lambda( exp ).Compile().DynamicInvoke() );
            var paramters = Is.Context.ToArray();
            Is.Context.Clear();

            return new UseCtorToCreateParamter<T>( paramters, newExpression.Constructor, typeof( T ) );
        }

        public static UseCtorToCreateControl<T> CreateControl<T>( Guid id, Expression<Func<T>> ctor ) where T : IControl
        {
            var newExpression = ctor.Body as NewExpression;
            if ( newExpression == null ) { throw new ArgumentException(); }
            var paramters = GetArguments( newExpression.Arguments ).ToArray();
            return new UseCtorToCreateControl<T>( id, paramters, newExpression.Constructor );
        }

        public static CallActionOnCreatedObject Call( Expression<Action> action )
        {
            var methodCall = action.Body.As<MethodCallExpression>();
            var method = methodCall.Method;
            var methodCallOnObjectId = methodCall.Object.As<MemberExpression>().GetValue<IAction>().CommandId;
            var paramters = GetArguments( methodCall.Arguments ).ToArray();

            return new CallActionOnCreatedObject( methodCallOnObjectId, method, paramters );

        }


        public static CallAction Call<TOWner>( Guid id, Expression<Action> expression )
        {
            var methodCallExpression = expression.Body.As<MethodCallExpression>();
            var result = new CallAction( id,
                                        GetPath<TOWner>( methodCallExpression ),
                                        methodCallExpression.As<MethodCallExpression>().Method,
                                        GetArguments( methodCallExpression.As<MethodCallExpression>().Arguments ).ToArray() );
            return result;
        }

        private static MemberInfo[] GetPath<TOwner>( MethodCallExpression expression )
        {
            var path = new List<MemberInfo>();

            var member = expression.Object as MemberExpression;
            if ( member == null ) { throw new ArgumentException(); }

            while ( member.Type != typeof( TOwner ) )
            {
                path.Add( member.Member );
                if ( !( member.Expression is MemberExpression ) )
                {
                    // Bug 
                    break;
                }
                member = ( MemberExpression ) member.Expression;
            }

            return path.ToArray();
        }


        private static IEnumerable<IAction> GetArguments( IEnumerable<Expression> arguments )
        {
            Is.Context.Clear();
            arguments.ForEach( exp => Expression.Lambda( exp ).Compile().DynamicInvoke() );
            var paramters = Is.Context.ToArray();
            Is.Context.Clear();

            return paramters;
        }
    }
}
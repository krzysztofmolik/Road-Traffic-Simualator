using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands
{
    [Serializable]
    public class CallAction : IAction
    {
        private MemberInfo[] _propertiesPath;
        private Guid _ownerId;
        private IParameter[] _parameters;
        private MethodInfo _methodInfo;

        public static CallAction Create<TOWner>( Guid id, Expression<Action> methodCallExpression, params IParameter[] parameters )
        {
            var result = new CallAction();
            result._propertiesPath = GetPath<TOWner>( methodCallExpression );
            result._methodInfo = GetMethodInfo( methodCallExpression );
            result._parameters = parameters;
            result._ownerId = id;
            return result;
        }

        private static MethodInfo GetMethodInfo( Expression expression )
        {
            var callExpression = ( ( Expression<Action> ) expression ).Body as MethodCallExpression;
            if ( callExpression == null ) { throw new ArgumentException(); }

            return callExpression.Method;
        }

        private static MemberInfo[] GetPath<TOwner>( Expression properties )
        {
            var path = new List<MemberInfo>();

            var expression = ( ( Expression<Action> ) properties ).Body as MethodCallExpression;
            if ( expression == null ) { throw new ArgumentException(); }

            var member = expression.Object as MemberExpression;
            if ( member == null ) { throw new ArgumentException(); }

            while ( member.Type != typeof( TOwner ) )
            {
                path.Add( member.Member );
                member = ( MemberExpression ) member.Expression;
            }

            return path.ToArray();
        }

        public void Execute( DeserializationContext context )
        {
            object owner = context.GetById( this._ownerId );
            foreach ( var memberInfo in this._propertiesPath.Reverse() )
            {
                if ( memberInfo is PropertyInfo )
                {
                    owner = ( ( PropertyInfo ) memberInfo ).GetValue( owner, null );
                }
                else if ( memberInfo is FieldInfo )
                {
                    owner = ( ( FieldInfo ) memberInfo ).GetValue( owner );
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            var parameters = this._parameters.Select( s => s.GetValue( context ) ).ToArray();
            this._methodInfo.Invoke( owner, parameters );
        }

        public Order Priority
        {
            get { return Order.Low; }
        }
    }
}
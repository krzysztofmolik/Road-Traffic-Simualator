using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands
{
    [Serializable]
    public class SetProperties<T> : IAction
    {
        private MemberInfo[] _propertiesPath;
        private T _value;
        private Guid _ownerId;

        public static SetProperties<T> Create<TOWner>( Guid id, Expression<Func<T>> properties )
        {
            var result = new SetProperties<T>();
            result._propertiesPath = GetPath<TOWner>( properties );
            result._value = GetValue( properties.Body );
            result._ownerId = id;
            return result;
        }

        private static T GetValue( Expression properties )
        {
            var lambda = Expression.Lambda<Func<T>>( properties ).Compile();
            return lambda();
        }

        private static MemberInfo[] GetPath<TOwner>( Expression properties )
        {
            var members = new List<MemberInfo>();
            var expression = ( ( Expression<Func<T>> ) properties ).Body as MemberExpression;
            if ( expression == null ) { throw new ArgumentException(); }

            members.Add( expression.Member );
            while ( expression.Expression is MemberExpression && expression.Member.ReflectedType != typeof( TOwner ) )
            {
                var innerExpression = ( MemberExpression ) expression.Expression;
                members.Add( innerExpression.Member );
                expression = innerExpression;
            }

            return members.ToArray();
        }

        public void Execute( DeserializationContext context )
        {
            object owner = context.GetById( this._ownerId );
            foreach ( var memberInfo in this._propertiesPath.Skip( 1 ).Reverse() )
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

            var last = this._propertiesPath.First() as PropertyInfo;
            Debug.Assert( last != null );
            last.SetValue( owner, this._value, null );
        }

        public Order Priority
        {
            get { return Order.Normal; }
        }
    }
}
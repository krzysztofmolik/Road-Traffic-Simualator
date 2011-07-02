using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Autofac;
using System.Linq;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands
{
    [Serializable]
    public class IocParameter : IParameter
    {
        private readonly Type _type;

        public static IocParameter Create<T>()
        {
            return new IocParameter( typeof( T ) );
        }

        public IocParameter( Type type )
        {
            this._type = type;
        }

        public object GetValue( DeserializationContext context )
        {
            return context.IoC.Resolve( this._type );
        }

        public Type Type
        {
            get { return _type; }
        }
    }

    [Serializable]
    public class ControlProperties : IParameter
    {
        private readonly Type _type;
        private readonly Guid _ownerId;
        private readonly MemberInfo _memberInfo;

        public static ControlProperties Create<TOwner, TProperty>( TOwner owner, TProperty propertyValue ) where TOwner : IControl
        {
            var property = owner.GetType().GetProperties().Where( s => s.PropertyType.IsAssignableFrom( propertyValue.GetType() ) ).FirstOrDefault( s => s.GetValue( owner, null ) == ( object ) propertyValue );
            return new ControlProperties( typeof( TProperty ), owner.Id, property );
        }

        public static ControlProperties Create<T>( Guid ownerId, Expression<Func<T>> expression )
        {
            var memeberExpression = expression.Body as MemberExpression;
            if ( memeberExpression == null ) { throw new ArgumentException(); }
            return new ControlProperties( typeof( T ), ownerId, memeberExpression.Member );
        }

        public ControlProperties( Type type, Guid ownerId, MemberInfo memberInfo )
        {
            Contract.Requires( type != null );
            Contract.Requires( memberInfo != null );
            this._type = type;
            this._ownerId = ownerId;
            this._memberInfo = memberInfo;
        }

        public object GetValue( DeserializationContext context )
        {
            var owner = context.GetById( this._ownerId );
            if ( this._memberInfo is PropertyInfo )
            {
                return ( ( PropertyInfo ) this._memberInfo ).GetValue( owner, null );
            }
            return ( ( FieldInfo ) this._memberInfo ).GetValue( owner );
        }

        public Type Type
        {
            get { return this._type; }
        }
    }
}
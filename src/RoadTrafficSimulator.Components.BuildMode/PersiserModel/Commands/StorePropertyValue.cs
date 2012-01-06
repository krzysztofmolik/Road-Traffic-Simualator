using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands
{
    [Serializable]
    public class StorePropertyValue : IAction
    {
        private readonly Type _type;
        private readonly Guid _ownerId;
        private readonly MemberInfo[] _path;
        private readonly object _value;
        private readonly Guid _commandId = Guid.NewGuid();

        public StorePropertyValue( Type type, Guid ownerId, MemberInfo[] path, object value )
        {
            Contract.Requires( type != null );
            Contract.Requires( path != null );
            this._type = type;
            this._ownerId = ownerId;
            this._path = path;
            this._value = value;
        }

        public object Execute( DeserializationContext context )
        {
            object result = context.GetById( this._ownerId );

            var reversePath = this._path.Reverse().ToArray();
            result = reversePath.Take( this._path.Length - 1 ).Aggregate( result, ( current, memberInfo ) => this.GetValue( memberInfo, current ) );
            this.SetValue( reversePath.Last(), result, this._value );
            return null;
        }

        private void SetValue( MemberInfo memberInfo, object owner, object value )
        {
            if ( memberInfo is PropertyInfo )
            {
                ( ( PropertyInfo ) memberInfo ).SetValue( owner, value, null );
                return;
            }
            ( ( FieldInfo ) memberInfo ).SetValue( owner, value );
        }

        private object GetValue( MemberInfo memberInfo, object owner )
        {
            if ( memberInfo is PropertyInfo )
            {
                return ( ( PropertyInfo ) memberInfo ).GetValue( owner, null );
            }
            return ( ( FieldInfo ) memberInfo ).GetValue( owner );
        }

        public Order Priority
        {
            get { return Order.Low; }
        }

        public Type Type
        {
            get { return this._type; }
        }

        public Guid CommandId
        {
            get { return this._commandId; }
        }
    }
}
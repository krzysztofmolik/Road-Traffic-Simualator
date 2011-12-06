using System;
using System.Linq;
using System.Reflection;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands
{
    [Serializable]
    public class UseCtorToCreateParamter<T> : IAction
    {
        private readonly IAction[] _parameters;
        private readonly ConstructorInfo _constructor;
        private readonly Type _type;
        private readonly Guid _id = Guid.NewGuid();

        public UseCtorToCreateParamter( IAction[] parameters, ConstructorInfo constructor, Type type )
        {
            this._parameters = parameters;
            this._constructor = constructor;
            this._type = type;
        }

        public Order Priority
        {
            get { return Order.Normal; }
        }

        public object Execute( DeserializationContext context )
        {
            var paramters = this._parameters.Select( p => p.Execute( context ) ).ToArray();
            var value = ( T ) this._constructor.Invoke( paramters );
            context.CreatedObjects.Add( this._id, value );
            return value;
        }

        public Type Type
        {
            get { return this._type; }
        }

        public Guid CommandId
        {
            get { return this._id; }
        }

        public T Default
        {
            get { return default( T ); }
            set { throw new NotImplementedException(); }
        }
    }
}
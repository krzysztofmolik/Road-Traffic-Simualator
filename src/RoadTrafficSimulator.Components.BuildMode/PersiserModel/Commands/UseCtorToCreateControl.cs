using System;
using System.Linq;
using System.Reflection;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands
{
    [Serializable]
    public class UseCtorToCreateControl<T> : IAction where T : IControl
    {
        private readonly Guid _id;
        private readonly IAction[] _parameters;
        private readonly ConstructorInfo _constructor;
        private readonly Guid _commandId = Guid.NewGuid();

        public UseCtorToCreateControl( Guid id, IAction[] parameters, ConstructorInfo constructor )
        {
            this._id = id;
            this._parameters = parameters;
            this._constructor = constructor;
        }

        public object Execute( DeserializationContext context )
        {
            var paramters = this._parameters.Select( p => p.Execute( context ) ).ToArray();
            var control = ( T ) this._constructor.Invoke( paramters );
            control.Id = this._id;
            context.CreateControls.Add( control );
            return control;
        }

        public Order Priority
        {
            get { return Order.High; }
        }

        public Type Type
        {
            get { return typeof(T); }
        }

        public Guid CommandId
        {
            get { return this._commandId; }
        }
    }
}
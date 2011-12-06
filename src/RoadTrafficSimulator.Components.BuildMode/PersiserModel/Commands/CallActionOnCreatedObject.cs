using System;
using System.Reflection;
using RoadTrafficSimulator.Infrastructure;
using System.Linq;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands
{
    [Serializable]
    public class CallActionOnCreatedObject : IAction
    {
        private readonly Guid _instanceId;
        private readonly MethodInfo _method;
        private readonly IAction[] _parameters;
        private readonly Guid _commandId = Guid.NewGuid();

        public CallActionOnCreatedObject( Guid instanceId, MethodInfo method, IAction[] paramters )
        {
            this._instanceId = instanceId;
            this._method = method;
            this._parameters = paramters;
        }

        public object Execute( DeserializationContext context )
        {
            var instance = context.CreatedObjects.Get( this._instanceId );
            var parameters = this._parameters.Select( par => par.Execute( context ) ).ToArray();

            return this._method.Invoke( instance, parameters );
        }

        public Order Priority
        {
            get { return Order.Low; }
        }

        public Type Type
        {
            get { return this._method.ReturnType; }
        }

        public Guid CommandId
        {
            get { return this._commandId; }
        }
    }
}
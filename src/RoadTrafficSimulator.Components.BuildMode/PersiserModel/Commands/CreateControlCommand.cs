using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands
{
    [Serializable]
    public class CreateControlCommand : IAction
    {
        private readonly Type _type;
        private readonly List<IAction> _parameters = new List<IAction>();
        private readonly Guid _objectId;
        private readonly Guid _commandId = Guid.NewGuid();

        public CreateControlCommand( Guid id, Type type )
        {
            this._objectId = id;
            this._type = type;
        }

        public void AddConstructParameters( IAction parameter )
        {
            this._parameters.Add( parameter );
        }

        public object Execute( DeserializationContext context )
        {
            var types = this._parameters.Select( s => s.Type ).ToArray();
            var constructor = this._type.GetConstructor( types );
            Debug.Assert( constructor != null );

            var parameters = this._parameters.Select( s => s.Execute( context ) ).ToArray();
            var obj = ( IControl ) constructor.Invoke( parameters );
            obj.Id = this._objectId;
            context.CreateControls.Add( obj );
            return obj;
        }

        public Order Priority { get { return Order.High; } }

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
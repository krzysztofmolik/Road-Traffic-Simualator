using System;
using System.Linq;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands
{
    [Serializable]
    public class ResolveBaseOnId<T> : IAction
    {
        private readonly Guid _id;
        private readonly Guid _propertyId = Guid.NewGuid();

        public ResolveBaseOnId( Guid id )
        {
            this._id = id;
        }

        public object Execute( DeserializationContext context )
        {
            return ( T ) context.CreateControls.First( s => s.Id == this._id );
        }

        public Order Priority
        {
            get { return Order.Low; }
        }

        public Type Type
        {
            get { return typeof(T); }
        }

        public Guid CommandId
        {
            get { return this._propertyId; }
        }
    }
}
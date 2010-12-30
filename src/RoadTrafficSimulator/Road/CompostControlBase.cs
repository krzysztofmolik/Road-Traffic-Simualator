using System;
using System.Collections.Generic;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;

namespace RoadTrafficSimulator.Road
{
    public abstract class CompostControlBase<TVertex> : ControlBaseBase<TVertex>, ICompostControlBase
    {
        private readonly object _synchronizationObject = new object();
        private readonly IList<IControl> _childrens = new List<IControl>();

        protected CompostControlBase( IControl parent )
            : base( parent )
        {
        }

        public virtual IEnumerable<IControl> Children { get { return this._childrens; } }

        public abstract IConnectionCompositeSupport ConnectionSupport { get; }

        public void AddChild( IControl singleControlBase )
        {
            lock ( this._synchronizationObject )
            {
                this._childrens.Add( singleControlBase );
            }

            singleControlBase.Changed.Subscribe( s => this.NotifyAboutChanged() );
        }

        public void RemoveChild( ISingleControl singleControlBase )
        {
            lock ( this._synchronizationObject )
            {
                this._childrens.Remove( singleControlBase );
            }
        }
    }
}
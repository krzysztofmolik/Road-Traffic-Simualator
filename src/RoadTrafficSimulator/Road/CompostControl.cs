using System;
using System.Collections.Generic;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;

namespace RoadTrafficSimulator.Road
{
    public abstract class CompostControl<TVertex> : ControlBaseBase<TVertex>, ICompositeControl
    {
        private readonly object _synchronizationObject = new object();
        private readonly IList<IControl> _childrens = new List<IControl>();

        protected CompostControl( IControl parent )
            : base( parent )
        {
        }

        public virtual IEnumerable<IControl> Children
        {
            get { return this._childrens; }
        }

        public void AddChild( IControl singleControlBase )
        {
            lock ( this._synchronizationObject )
            {
                this._childrens.Add( singleControlBase );
            }

            singleControlBase.Changed.Subscribe( s => this.Invalidate() );
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
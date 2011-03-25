using System;
using System.Collections.Generic;
using RoadTrafficSimulator.Infrastructure.Control;
using System.Linq;

namespace RoadTrafficSimulator.Road
{
    public abstract class CompostControl<TVertex> : ControlBaseBase<TVertex>, ICompositeControl
    {
        private readonly object _synchronizationObject = new object();
        private readonly IList<IControl> _childrens = new List<IControl>();

        public virtual IEnumerable<IControl> Children
        {
            get { return this._childrens; }
        }

        public void AddChild( IControl control )
        {
            lock ( this._synchronizationObject )
            {
                this._childrens.Add( control );
            }

            control.Translated.Subscribe( s => this.OnChildrenTranslated() );
            control.Redrawed.Subscribe( s => this.OnChildrenRedrawed() );
        }

        protected virtual void OnChildrenTranslated()
        { }

        protected virtual void OnChildrenRedrawed()
        {
            this.Redraw();
        }

        public void RemoveChild( ISingleControl singleControlBase )
        {
            // TODO Implement it
        }

        public override ILogicControl GetHittedControl( Microsoft.Xna.Framework.Vector2 point )
        {
            var control = this._childrens.Select( c => c.GetHittedControl( point ) ).Where( c => c != null ).FirstOrDefault();
            if ( control != null )
            {
                return control;
            }

            return base.GetHittedControl( point );
        }
    }
}
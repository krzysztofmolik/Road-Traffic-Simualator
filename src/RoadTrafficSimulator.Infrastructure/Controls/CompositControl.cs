using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using XnaVs10.MathHelpers;

namespace RoadTrafficSimulator.Infrastructure.Controls
{
    public abstract class CompositControl<TVertex> : ControlBaseBase<TVertex>, ICompositeControl
    {
        private readonly IList<IControl> _childrens = new List<IControl>();

        public virtual IEnumerable<IControl> Children
        {
            get { return this._childrens; }
        }

        public virtual void AddChild( IControl control )
        {
            this._childrens.Add( control );

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

        public override bool IsHitted( Vector2 location )
        {
            return HitTestAlghoritm.HitTest( location, this.VertexContainer.Shape.ShapePoints );
        }
    }
}
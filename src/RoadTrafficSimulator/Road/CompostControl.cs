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

        public void AddChild( IControl singleControlBase )
        {
            lock ( this._synchronizationObject )
            {
                this._childrens.Add( singleControlBase );
            }

            singleControlBase.Translated.Subscribe( s => this.TranslatedSubject.OnNext( new TranslationChangedEventArgs( this ) ) );
        }

        public void RemoveChild( ISingleControl singleControlBase )
        {
            lock ( this._synchronizationObject )
            {
                this._childrens.Remove( singleControlBase );
            }
        }

        public override ILogicControl HitTest( Microsoft.Xna.Framework.Vector2 point )
        {
            var control = this._childrens.Select( c => c.HitTest( point ) ).Where( c => c != null ).FirstOrDefault();
            if( control != null )
            {
                return control;
            }

            return base.HitTest( point );
        }
    }
}
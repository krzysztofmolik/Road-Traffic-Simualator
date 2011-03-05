using System;
using System.Collections.Generic;
using RoadTrafficSimulator.Infrastructure.Control;

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
    }
}
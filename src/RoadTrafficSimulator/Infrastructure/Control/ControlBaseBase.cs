using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Mouse;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using XnaRoadTrafficConstructor.Infrastucure.Mouse;
using XnaVs10.MathHelpers;

namespace RoadTrafficSimulator.Infrastructure.Control
{
    public abstract class ControlBaseBase<TVertex> : IControl
    {
        private readonly ISubject<bool> _isSelectedChanged = new Subject<bool>();
        private readonly IList<IControl> _parents;
        private bool _isSelected;

        protected ControlBaseBase( IControl parent )
        {
            this._parents = new List<IControl> { parent };
            this.ChangedSubject = new Subject<Unit>();
        }

        public abstract IVertexContainer<TVertex> SpecifiedVertexContainer { get; }

        public IVertexContainer VertexContainer { get { return this.SpecifiedVertexContainer; } }

        public abstract IMouseSupport MouseSupport { get; }

        public abstract Vector2 Location { get; }

        public abstract ISelectionSupport SelectionSupport { get; }

        public bool IsSelected
        {
            get { return this._isSelected; }
            set
            {
                this._isSelected = value;
                this._isSelectedChanged.OnNext( value );
                this.ChangedSubject.OnNext( new Unit() );
            }
        }

        public IObservable<Unit> Changed { get { return this.ChangedSubject; } }

        public IEnumerable<IControl> Parents
        {
            get { return this._parents; }
        }

        public IObservable<bool> IsSelectedChanged
        {
            get { return this._isSelectedChanged; }
        }

        protected ISubject<Unit> ChangedSubject { get; private set; }

        public abstract void Translate( Matrix matrixTranslation );

        public IControl GetRoot()
        {
            IControl root = this;
            while ( root.Parents != null )
            {
                var newRoot = root.Parents.FirstOrDefault();
                if ( newRoot == null )
                {
                    return root;
                }
                root = newRoot;
            }

            return root;
        }

        public virtual Vector2 ToControlPosition( Vector2 screenCordination )
        {
            return this.Location - screenCordination;
        }

        public virtual bool HitTest( Vector2 point )
        {
            return HitTestAlghoritm.HitTest( point, this.VertexContainer.Shape.ShapePoints );
        }

        public void AddParent( IControl controlBase )
        {
            if ( this._parents.Contains( controlBase ) )
            {
                return;
            }

            this._parents.Add( controlBase );
        }

        protected virtual void NotifyAboutChanged()
        {
            this.ChangedSubject.OnNext( new Unit() );
        }
    }
}
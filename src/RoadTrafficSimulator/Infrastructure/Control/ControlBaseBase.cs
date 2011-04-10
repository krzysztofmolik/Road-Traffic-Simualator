using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Mouse;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using XnaVs10.MathHelpers;

namespace RoadTrafficSimulator.Infrastructure.Control
{
    public abstract class ControlBaseBase<TVertex> : IControl
    {
        private readonly ISubject<bool> _selectionChanged = new Subject<bool>();
        private readonly ISubject<Unit> _redrawEvent = new Subject<Unit>();
        private readonly ISubject<TranslationChangedEventArgs> _translatedSubject = new Subject<TranslationChangedEventArgs>();
        private bool _isSelected;

        public abstract IVertexContainer VertexContainer { get; } 
        public abstract IMouseHandler MouseHandler { get; }
        public abstract Vector2 Location { get; }
        public abstract IControl Parent { get; }

        public bool IsSelected
        {
            get { return this._isSelected; }
            set
            {
                this._isSelected = value;
                this._selectionChanged.OnNext( value );
                this.Redraw();
            }
        }

        public IObservable<TranslationChangedEventArgs> Translated { get { return this._translatedSubject; } }

        public IObservable<Unit> Redrawed { get { return this._redrawEvent; } }

        public IObservable<bool> SelectionChanged
        {
            get { return this._selectionChanged; }
        }

        public void Invalidate()
        {
            this.OnInvalidate();
            this.Redraw();
        }

        public virtual void Redraw()
        {
            this.OnRedraw();
        }

        public abstract void Translate( Matrix matrixTranslation );

        public abstract void TranslateWithoutNotification(Matrix translationMatrix);

        public virtual bool IsHitted( Vector2 location )
        {
            return HitTestAlghoritm.HitTest( location, this.VertexContainer.Shape.ShapePoints );
        }

        public virtual ILogicControl GetHittedControl(Vector2 point)
        {
            if ( HitTestAlghoritm.HitTest( point, this.VertexContainer.Shape.ShapePoints ) )
            {
                return this;
            }
            return null;
        }

        public virtual Vector2 ToControlPosition( Vector2 screenCordination )
        {
            return this.Location - screenCordination;
        }

        protected virtual void OnRedraw()
        {
            this._redrawEvent.OnNext( new Unit() );
        }

        protected virtual void OnInvalidate()
        {
            this._translatedSubject.OnNext( new TranslationChangedEventArgs( this ) );
        }
    }
}
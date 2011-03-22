﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Mouse;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using XnaVs10.MathHelpers;

namespace RoadTrafficSimulator.Infrastructure.Control
{
    public abstract class ControlBaseBase<TVertex> : IControl
    {
        private readonly ISubject<bool> _isSelectedChanged = new Subject<bool>();
        private bool _isSelected;
        private readonly ISubject<Unit> _redrawEvent = new Subject<Unit>();

        protected ControlBaseBase()
        {
            this.TranslatedSubject = new Subject<TranslationChangedEventArgs>();
        }

        public abstract IVertexContainer<TVertex> SpecifiedVertexContainer { get; }

        public IVertexContainer VertexContainer { get { return this.SpecifiedVertexContainer; } }

        public abstract IMouseSupport MouseSupport { get; }

        public abstract Vector2 Location { get; }

        public abstract IControl Parent { get; }

        public bool IsSelected
        {
            get { return this._isSelected; }
            set
            {
                this._isSelected = value;
                this._isSelectedChanged.OnNext( value );
                this.TranslatedSubject.OnNext( new TranslationChangedEventArgs( this ) );
            }
        }

        public IObservable<TranslationChangedEventArgs> Translated { get { return this.TranslatedSubject; } }

        public IObservable<Unit> Redrawed { get { return this._redrawEvent; } }

        public IObservable<bool> IsSelectedChanged
        {
            get { return this._isSelectedChanged; }
        }

        public void Invalidate()
        {
            this.OnInvalidate();
        }

        protected virtual void OnInvalidate()
        {
            this._redrawEvent.OnNext( new Unit() );
        }

        protected virtual void OnTranslate()
        {
            this.TranslatedSubject.OnNext( new TranslationChangedEventArgs( this ) );
        }

        protected ISubject<TranslationChangedEventArgs> TranslatedSubject { get; private set; }

        public abstract void Translate( Matrix matrixTranslation );

        public IControl GetRoot()
        {
            IControl root = this;
            while ( root.Parent != null )
            {
                var newRoot = root.Parent;
                root = newRoot;
            }

            return root;
        }

        public virtual bool IsHitted( Vector2 location )
        {
            return HitTestAlghoritm.HitTest( location, this.VertexContainer.Shape.ShapePoints );
        }

        public virtual Vector2 ToControlPosition( Vector2 screenCordination )
        {
            return this.Location - screenCordination;
        }

        public virtual ILogicControl HitTest( Vector2 point )
        {
            if ( HitTestAlghoritm.HitTest( point, this.VertexContainer.Shape.ShapePoints ) )
            {
                return this;
            }
            return null;
        }

        public virtual void Update()
        {
            this.OnTranslate();
        }
    }
}
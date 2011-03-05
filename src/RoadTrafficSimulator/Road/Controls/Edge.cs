using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.VertexContainers;
using XnaRoadTrafficConstructor.Infrastucure.Draw;

namespace RoadTrafficSimulator.Road.Controls
{
    public abstract class Edge : CompostControl<VertexPositionColor>
    {
        private readonly EdgeVertexContainer _specifiedVertexContainer;
        private readonly IMouseSupport _mouseSupport;
        private float _width;
        private MovablePoint _startPoint;
        private MovablePoint _endPoint;

        protected Edge( Factories.Factories factories )
        {
            this._specifiedVertexContainer = new EdgeVertexContainer( this );
            this._mouseSupport = new CompositeControlMouseSupport( this );
            this._startPoint = new MovablePoint( factories, Vector2.Zero, this );
            this._endPoint = new MovablePoint( factories, Vector2.Zero, this );
            this.AddChild( this._startPoint );
            this.AddChild( this._endPoint );
        }

        protected Edge( Factories.Factories factories, MovablePoint startPoint, MovablePoint endPoint, float width )
            : this( factories )
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
            this._width = width;
        }

        public Vector2 StartLocation
        {
            get { return this._startPoint.Location; }
        }

        public Vector2 EndLocation
        {
            get { return this._endPoint.Location; }
        }

        public MovablePoint StartPoint
        {
            get { return this._startPoint; }
            set
            {
                this.RemoveChild( this._startPoint );
                this._startPoint = value;
                this.AddChild( value );
                this.TranslatedSubject.OnNext( new TranslationChangedEventArgs( this ) );
            }
        }

        public MovablePoint EndPoint
        {
            get { return this._endPoint; }
            set
            {
                this.RemoveChild( this._endPoint );
                this._endPoint = value;
                this.AddChild( value );
                this.TranslatedSubject.OnNext( new TranslationChangedEventArgs( this ) );
            }
        }

        public float Width
        {
            get { return this._width; }
            set
            {
                this._width = value;
                this.TranslatedSubject.OnNext( new TranslationChangedEventArgs( this ) );
            }
        }

        public override IVertexContainer<VertexPositionColor> SpecifiedVertexContainer
        {
            get { return this._specifiedVertexContainer; }
        }

        public override IMouseSupport MouseSupport
        {
            get { return this._mouseSupport; }
        }

        public override Vector2 Location
        {
            get { return this.StartLocation + ( ( this.EndLocation - this.StartLocation ) / 2 ); }
        }

        public override void Translate( Matrix matrixTranslation )
        {
            var startPoint = this.StartPoint.Location;
            var endPoint = this.EndPoint.Location;
            this.StartPoint.Translate( matrixTranslation );
            this.EndPoint.Translate( matrixTranslation );

            if ( startPoint != this.StartPoint.Location || endPoint != this.EndPoint.Location )
            {
                this.OnTranslated();
            }
        }

        protected virtual void OnTranslated()
        {
            this.TranslatedSubject.OnNext( new TranslationChangedEventArgs( this ) );
        }
    }
}
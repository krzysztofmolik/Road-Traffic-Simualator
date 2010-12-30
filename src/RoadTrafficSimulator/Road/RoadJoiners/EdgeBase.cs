using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.VertexContainers;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using XnaRoadTrafficConstructor.Infrastucure.Mouse;
using XnaRoadTrafficConstructor.Road;
using XnaRoadTrafficConstructor.Road.RoadJoiners;
using XnaRoadTrafficConstructor.VertexContainers;

namespace RoadTrafficSimulator.Road.RoadJoiners
{
    public abstract class EdgeBase : CompostControlBase<VertexPositionColor>
    {
        private readonly EdgeVertexContainer _specifiedVertexContainer;
        private readonly IMouseSupport _mouseSupport;
        private readonly ISelectionSupport _selectionSupport;
        private readonly IConnectionCompositeSupport _connectionSupport;
        private float _width;
        private MovablePoint _startPoint;
        private MovablePoint _endPoint;

        protected EdgeBase( IControl parent )
            : base( parent )
        {
            this._specifiedVertexContainer = new EdgeVertexContainer( this );
            this._mouseSupport = new CompositeControlMouseSupport( this );
            this._startPoint = new MovablePoint( Vector2.Zero, this );
            this._endPoint = new MovablePoint( Vector2.Zero, this );
            this._selectionSupport = new DefaultCompositeControlSelectionSupport( this );
            this._connectionSupport = new CompositeConnectionSupport<EdgeBase>( this );
            this.AddChild( this._startPoint );
            this.AddChild( this._endPoint );
        }

        protected EdgeBase( MovablePoint startPoint, MovablePoint endPoint, float width, IControl parent )
            : this( parent )
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
            this._width = width;
            this._startPoint.LocationChanged.Subscribe( s => this.ChangedSubject.OnNext( new Unit() ) );
            this._endPoint.LocationChanged.Subscribe( s => this.ChangedSubject.OnNext( new Unit() ) );
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
                this._startPoint.AddParent( this );
                this.AddChild( value );
                this.ChangedSubject.OnNext( new Unit() );
            }
        }

        public MovablePoint EndPoint
        {
            get { return this._endPoint; }
            set
            {
                this.RemoveChild( this._endPoint );
                this._endPoint = value;
                this._endPoint.AddParent( this );
                this.AddChild( value );
                this.ChangedSubject.OnNext( new Unit() );
            }
        }

        public float Width
        {
            get { return this._width; }
            set
            {
                this._width = value;
                this.ChangedSubject.OnNext( new Unit() );
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

        public override IConnectionCompositeSupport ConnectionSupport
        {
            get { return this._connectionSupport; }
        }

        public override Vector2 Location
        {
            get { return this.StartLocation + ( ( this.EndLocation - this.StartLocation ) / 2 ); }
        }

        public override ISelectionSupport SelectionSupport
        {
            get { return this._selectionSupport; }
        }

        public override void Translate( Matrix matrixTranslation )
        {
            this.StartPoint.Translate( matrixTranslation );
            this.EndPoint.Translate( matrixTranslation );
        }
    }
}
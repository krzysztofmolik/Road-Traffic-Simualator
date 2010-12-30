using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.VertexContainers;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using XnaRoadTrafficConstructor.Infrastucure.Mouse;
using XnaVs10.Extension;

namespace XnaRoadTrafficConstructor.Road.RoadJoiners
{
    public class MovablePoint : SingleControl<VertexPositionColor>
    {
        private readonly MovablePointVertexContainer _movablePointVertexContainer;
        private readonly IMouseSupport _mouseSupport;
        private readonly ISelectionSupport _selectionSupport;
        private readonly IConnectionSupport _connectionSupport;
        private Vector2 _location;

        public MovablePoint( Vector2 location, IControl parent )
            : base( parent )
        {
            this._mouseSupport = new ControlMouseSupport( this );
            this.LocationChanged = new Subject<Vector2>();
            this._location = location;
            this._movablePointVertexContainer = new MovablePointVertexContainer( this );
            this._selectionSupport = new DefaultControlSelectionSupport( this );
            this._connectionSupport = new EmptyConnectionSupport<MovablePoint>( this );
        }

        public override IConnectionSupport ConnectionSupport
        {
            get { return this._connectionSupport; }
        }

        public override Vector2 Location
        {
            get { return _location;  }
        }

        public override ISelectionSupport SelectionSupport
        {
            get { return this._selectionSupport; }
        }

        public ISubject<Vector2> LocationChanged { get; private set; }

        public override IVertexContainer<VertexPositionColor> SpecifiedVertexContainer
        {
            get { return this._movablePointVertexContainer; }
        }

        public override IMouseSupport MouseSupport
        {
            get { return this._mouseSupport; }
        }

        public override void Translate( Matrix matrixTranslation )
        {
            this._location = Vector2.Transform( this.Location, matrixTranslation );
            this.NotifyAboutChanged();
        }

        public void SetLocation( Vector2 newLocation )
        {
            if ( this.Location == newLocation )
            {
                return;
            }

            var diff = newLocation - this.Location;
            this.Translate( Matrix.CreateTranslation( diff.ToVector3() ) );
        }
    }
}
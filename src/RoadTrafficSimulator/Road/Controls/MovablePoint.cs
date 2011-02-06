using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.VertexContainers;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using XnaVs10.Extension;

namespace RoadTrafficSimulator.Road.Controls
{
    public class MovablePoint : SingleControl<VertexPositionColor>
    {
        private readonly MovablePointVertexContainer _movablePointVertexContainer;
        private readonly IMouseSupport _mouseSupport;
        private readonly IControl _parent;
        private Vector2 _location;

        public MovablePoint( Factories.Factories factories, Vector2 location, IControl parent )
        {
            this._parent = parent;
            this._mouseSupport = new ControlMouseSupport( this );
            this.LocationChanged = new Subject<Vector2>();
            this._location = location;
            this._movablePointVertexContainer = new MovablePointVertexContainer( this );
        }

        public override Vector2 Location
        {
            get { return this._location; }
        }

        public override IControl Parent
        {
            get { return this._parent; }
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
            this.ChangedSubject.OnNext( new Unit() );
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
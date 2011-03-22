using System;
using Common.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NLog;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.VertexContainers;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using XnaRoadTrafficConstructor.Road;
using XnaVs10.Extension;

namespace RoadTrafficSimulator.Road.Controls
{
    public class MovablePoint : SingleControl<VertexPositionColor>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly MovablePointVertexContainer _movablePointVertexContainer;
        private readonly IMouseSupport _mouseSupport;
        private readonly IControl _parent;
        private Vector2 _location;

        public MovablePoint( Factories.Factories factories, Vector2 location, IControl parent )
        {
            this._parent = parent;
            this._mouseSupport = new ControlMouseSupport( this );
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
            var newLocation = Vector2.Transform( this.Location, matrixTranslation );
            if ( newLocation == this._location )
            {
                Logger.Warn( "Given location is not valid, newLocation = {0}", newLocation );
                return;
            }

            this._location = newLocation;
            this.OnTranslate();
        }

        public void SetLocationWithoutEvent( Vector2 newLocation )
        {
            if ( newLocation.IsValid() == false )
            {
                Logger.Warn( "Given location is not valid, newLocation = {0}", newLocation );
                return;
            }

            if ( this.Location.Equal( newLocation, Constans.Epsilon ) )
            {
                return;
            }

            this._location = newLocation;
        }

        public void SetLocation( Vector2 newLocation )
        {
            if ( newLocation.IsValid() == false )
            {
                throw new ArgumentException( "New location is not valid" );
            }

            if ( this.Location.Equal( newLocation, Constans.Epsilon ) )
            {
                return;
            }

            var diff = newLocation - this.Location;
            this.Translate( Matrix.CreateTranslation( diff.ToVector3() ) );
        }
    }
}
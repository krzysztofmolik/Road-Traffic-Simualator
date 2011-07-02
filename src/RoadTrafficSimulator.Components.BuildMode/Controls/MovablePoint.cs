using System;
using Common.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Components.BuildMode.VertexContainers;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Draw;
using RoadTrafficSimulator.Infrastructure.Extension;
using RoadTrafficSimulator.Infrastructure.Mouse;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class MovablePoint : SingleControl<VertexPositionColor>
    {
        private readonly MovablePointVertexContainer _movablePointVertexContainer;
        private readonly IMouseHandler _mouseHandler;
        private Vector2 _location;

        public MovablePoint( Factories.Factories factories, Vector2 location, IControl parent )
        {
            this.Parent = parent;
            this._mouseHandler = factories.MouseHandlerFactory.Create( this );
            this._location = location;
            this._movablePointVertexContainer = new MovablePointVertexContainer( this );
        }

        public override Vector2 Location
        {
            get { return this._location; }
            protected set
            {
                this._location = value;
                this.Invalidate();
            }
        }

        public override IControl Parent { get; set; }

        public override IVertexContainer VertexContainer
        {
            get { return this._movablePointVertexContainer; }
        }

        public override IMouseHandler MouseHandler
        {
            get { return this._mouseHandler; }
        }

        public void TranslateWithoutEvent( Matrix matrixTranslation )
        {
            var newLocation = Vector2.Transform( this.Location, matrixTranslation );
            if ( newLocation == this._location )
            {
                return;
            }

            this._location = newLocation;

        }

        public override void Translate( Matrix matrixTranslation )
        {
            this.TranslateWithoutNotification( matrixTranslation );
            this.Invalidate();
        }

        public override void TranslateWithoutNotification( Matrix translationMatrix )
        {
            var newLocation = Vector2.Transform( this.Location, translationMatrix );
            if ( newLocation == this._location )
            {
                return;
            }

            this._location = newLocation;
        }

        public bool SetLocation( Vector2 newLocation )
        {
            if ( newLocation.IsValid() == false )
            {
                throw new ArgumentException( "New location is not valid" );
            }

            if ( this.Location.Equal( newLocation, Constans.Epsilon ) )
            {
                return false;
            }

            var diff = newLocation - this.Location;
            this.Translate( Matrix.CreateTranslation( diff.ToVector3() ) );
            return true;
        }
    }
}
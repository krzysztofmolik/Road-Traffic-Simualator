using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Cars;
using RoadTrafficSimulator.Infrastructure;
using XnaRoadTrafficConstructor.Infrastucure.Draw;

namespace RoadTrafficSimulator.Road
{
    public class CarsComponet : DrawableGameComponent, IDisposable
    {
        private List<Car> _cars;
        private Graphic _graphics;

        public CarsComponet( IGraphicsDeviceService graphicsDeviceService, Graphic graphics )
            : base( graphicsDeviceService )
        {
            this._cars = new List<Car>();
            this._graphics = graphics;
        }

        protected override void UnloadContent()
        {
        }

        protected override void LoadContent()
        {
        }

        public override void Draw( Microsoft.Xna.Framework.GameTime gameTime )
        {
            this._cars.ForEach( s => s.VertexContainer.Draw( this._graphics ) );
            base.Draw( gameTime );
        }

        public void Dispose()
        {
        }
    }
}
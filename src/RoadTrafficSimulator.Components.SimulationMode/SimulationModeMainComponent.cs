using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.Draw;
using DrawableGameComponent = RoadTrafficSimulator.Infrastructure.DrawableGameComponent;

namespace RoadTrafficSimulator.Components.SimulationMode
{
    // TODO Change name and move to different namespace
    public class SimulationModeMainComponent : DrawableGameComponent
    {
        private List<IRoadElement> _roadElements = new List<IRoadElement>();
        private Graphic _graphic;

        public SimulationModeMainComponent( IGraphicsDeviceService graphicsDeviceService, Graphic graphic )
            : base( graphicsDeviceService )
        {
            Contract.Requires( graphic != null );
            this._graphic = graphic;
        }

        public void AddRoadElement( IRoadElement roadElement )
        {
            this._roadElements.Add( roadElement );
        }

        public override void Draw( GameTime gameTime )
        {
            this._roadElements.ForEach( s => s.Draw( this._graphic, gameTime ) );
            base.Draw( gameTime );
            this._graphic.VertexPositionalColorDrawer.Flush();
            this._graphic.VertexPositionalTextureDrawer.Flush();
        }

        public override void Update( GameTime time )
        {
            this._roadElements.ForEach( s => s.Update( time ) );
        }

        protected override void UnloadContent()
        {
        }

        protected override void LoadContent()
        {
        }
    }
}
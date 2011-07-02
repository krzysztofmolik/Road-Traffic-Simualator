using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Light;
using RoadTrafficSimulator.Infrastructure.Draw;

namespace RoadTrafficSimulator.Components.SimulationMode.Controlers
{
    public class LightsUpdateControler : IControlers
    {
        private readonly List<Light> _lights = new List<Light>();
        private readonly Graphic _graphic;

        public LightsUpdateControler( Graphic graphic )
        {
            Contract.Requires( graphic != null );
            this._graphic = graphic;
        }

        public void AddControl( IRoadElement element )
        {
            if ( element is Light )
            {
                this._lights.Add( ( Light ) element );
            }
        }

        public void Draw( GameTime gameTime )
        {
            this._lights.ForEach( light => this.Update( light, gameTime ) );
            this._graphic.VertexPositionalColorDrawer.Flush();
        }

        private void Update( Light light, GameTime gameTime )
        {
            light.StateMachine.Update( gameTime );
        }

        private void Draw( Light light, GameTime time )
        {
            light.Drawer.Draw( this._graphic, time );
        }

        public void Update( GameTime gameTime ) { }

        public int Order
        {
            get { return ( int ) Infrastructure.Order.High; }
        }
    }
}
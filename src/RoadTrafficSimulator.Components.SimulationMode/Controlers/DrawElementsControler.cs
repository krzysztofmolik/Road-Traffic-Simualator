using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Draw;

namespace RoadTrafficSimulator.Components.SimulationMode.Controlers
{
    public class DrawElementsControler : IControlers
    {
        private readonly List<IRoadElement> _elementsToDraw = new List<IRoadElement>();
        private readonly Graphic _graphic;

        public DrawElementsControler( Graphic graphic )
        {
            Contract.Requires( graphic != null );
            this._graphic = graphic;
        }

        public void AddControl( IRoadElement element )
        {
            this._elementsToDraw.Add( element );
        }

        public void Draw( GameTime gameTime )
        {
            this._elementsToDraw.ForEach( s => s.Drawer.Draw( this._graphic, gameTime ) );
            this._graphic.VertexPositionalColorDrawer.Flush();
            this._graphic.VertexPositionalTextureDrawer.Flush();
        }

        public void Update( GameTime gameTime )
        {
        }

        public int Order
        {
            get { return ( int ) Infrastructure.Order.Normal; }
        }
    }
}
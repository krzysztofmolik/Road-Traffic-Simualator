using Common;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Infrastructure.Draw;

namespace RoadTrafficSimulator.Components.SimulationMode
{
    public class LaneJunctionDrawer : IDrawer
    {
        private LaneJunction _owner;

        public LaneJunctionDrawer( LaneJunction owner )
        {
            this._owner = owner;
        }

        public void Draw( Graphic graphic, GameTime gameTime )
        {
            this._owner.BuildControl.VertexContainer.Draw( graphic );
            this._owner.Edges.ForEach( e => e.Drawer.Draw( graphic, gameTime ) );
        }
    }
}
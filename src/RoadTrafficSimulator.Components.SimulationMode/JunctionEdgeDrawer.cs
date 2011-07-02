using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Infrastructure.Draw;

namespace RoadTrafficSimulator.Components.SimulationMode
{
    public class JunctionEdgeDrawer : IDrawer
    {
        private readonly JunctionEdge _owner;

        public JunctionEdgeDrawer( JunctionEdge owner )
        {
            this._owner = owner;
        }

        public void Draw( Graphic graphic, GameTime gameTime )
        {
            this._owner.EdgeBuilder.VertexContainer.Draw( graphic );
        }
    }
}
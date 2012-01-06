using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Draw;

namespace RoadTrafficSimulator.Components.BuildMode.VertexContainers
{
    // TODO Use composition insted of inheritance
    public class JunctionEdgeVertexContainer : EdgeVertexContainer
    {
        private readonly JunctionEdge _junctionEdge;

        public JunctionEdgeVertexContainer( JunctionEdge edge, Style style )
            : base( edge.Edge, style.NormalColor )
        {
            this._junctionEdge = edge;
        }
    }
}
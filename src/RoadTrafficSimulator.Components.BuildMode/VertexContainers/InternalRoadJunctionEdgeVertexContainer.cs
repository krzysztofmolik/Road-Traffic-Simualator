using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.BuildMode.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.VertexContainers
{
    public class InternalRoadJunctionEdgeVertexContainer : EdgeVertexContainer
    {
        private readonly InternalRoadJunctionEdge _edge;

        public InternalRoadJunctionEdgeVertexContainer( InternalRoadJunctionEdge edge )
            : base( edge, Color.White )
        {
            this._edge = edge;
        }

        public override void ClearColor()
        {
            this.Color = this.GetColor();
        }

        private Color GetColor()
        {
            return Color.Black;
        }
    }
}
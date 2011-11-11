using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.BuildMode.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.VertexContainers
{
    public class RoadJunctionEdgeVertexContainer : EdgeVertexContainer
    {
        private readonly RoadJunctionEdge _edge;

        private readonly Color _whenConnected = Color.White * 0;

        public RoadJunctionEdgeVertexContainer( RoadJunctionEdge edge )
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
            if ( this._edge.Connector.AreAllSlotOccupied )
            {
                return this._whenConnected;
            }

            return Color.Black;
        }
    }
}
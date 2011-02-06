using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Road.Controls;

namespace RoadTrafficSimulator.VertexContainers
{
    public class RoadJunctionEdgeVertexContainer : EdgeVertexContainer
    {
        private readonly RoadJunctionEdge _edge;

        private readonly Color _whenConnected = Color.White * 0;

        public RoadJunctionEdgeVertexContainer( RoadJunctionEdge edge )
            : base( edge )
        {
            this._edge = edge;
        }

        protected override Color GetColor()
        {
            if ( this._edge.Connector.AreAllSlotOccupied )
            {
                return this._whenConnected;
            }

            return base.GetColor();
        }
    }
}
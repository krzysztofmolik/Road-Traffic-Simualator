using System.Diagnostics;
using System.Linq;
using RoadTrafficSimulator.Road.Connectors;

namespace RoadTrafficSimulator.Road.Controls
{
    public class RoadJunctionEdge : Edge
    {
        private readonly RoadJunctionEdgeConnector _roadJunctionEndConnector;

        public RoadJunctionEdge( RoadJunctionBlock parent )
            : base( parent )
        {
            this._roadJunctionEndConnector = new RoadJunctionEdgeConnector( this );
        }

        public RoadJunctionEdge( MovablePoint startPoint, MovablePoint endPoint, float width, RoadJunctionBlock parent )
            : base( startPoint, endPoint, width, parent )
        {
        }

        public RoadJunctionEdgeConnector Connector
        {
            get { return this._roadJunctionEndConnector; }
        }

        public RoadJunctionBlock RoadJunctionParent
        {
            get
            {
                var result = this.Parents.First() as RoadJunctionBlock;
                Debug.Assert( result != null, "result != null" );
                return result;
            }
        }
    }
}
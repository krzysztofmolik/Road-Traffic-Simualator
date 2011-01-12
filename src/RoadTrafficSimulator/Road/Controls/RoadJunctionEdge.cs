using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Road.Connectors;

namespace RoadTrafficSimulator.Road.Controls
{
    public class RoadJunctionEdge : Edge
    {
        private readonly RoadJunctionEdgeConnector _roadJunctionEndConnector;

        private readonly RoadJunctionBlock _parent;

        public RoadJunctionEdge( RoadJunctionBlock parent )
        {
            this._parent = parent;
            this._roadJunctionEndConnector = new RoadJunctionEdgeConnector( this );
        }

        public RoadJunctionEdge( MovablePoint startPoint, MovablePoint endPoint, float width, RoadJunctionBlock parent )
            : base( startPoint, endPoint, width)
        {
            this._parent = parent;
        }

        public RoadJunctionEdgeConnector Connector
        {
            get { return this._roadJunctionEndConnector; }
        }

        public RoadJunctionBlock RoadJunctionParent
        {
            get { return this._parent; }
        }

        public override IControl Parent
        {
            get { return this._parent; }
        }
    }
}
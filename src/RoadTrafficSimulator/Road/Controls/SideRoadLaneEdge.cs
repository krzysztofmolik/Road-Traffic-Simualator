using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Road.Connectors;
using RoadTrafficSimulator.VertexContainers;
using XnaRoadTrafficConstructor.Infrastucure.Draw;

namespace RoadTrafficSimulator.Road.Controls
{
    public class SideRoadLaneEdge : Edge
    {
        private readonly RoadLaneBlock _parent;
        private readonly SideRoadLaneEdgeVertexContainer _vertexContainer;
        private readonly SideRoadLaneConnector _connector;
        private LaneType _laneType;

        public SideRoadLaneEdge(Factories.Factories factories, MovablePoint startPoint, MovablePoint endPoint, RoadLaneBlock parent)
            : base( factories, startPoint, endPoint )
        {
            this._connector = new SideRoadLaneConnector( this );
            this._parent = parent;
            this._laneType = LaneType.SolidLine;
            this._vertexContainer = new SideRoadLaneEdgeVertexContainer( this );
        }

        public RoadLaneBlock RoadLaneBlockParent
        {
            get
            {
                return this._parent;
            }
        }

        public LaneType LaneType
        {
            get
            {
                return this._laneType;
            }

            set
            {
                this._laneType = value;
                this.Redraw();
            }
        }

        public override IVertexContainer VertexContainer
        {
            get { return this._vertexContainer; }
        }

        public override IControl Parent
        {
            get { return this._parent; }
        }

        public SideRoadLaneConnector Connector
        {
            get { return this._connector; }
        }
    }
}
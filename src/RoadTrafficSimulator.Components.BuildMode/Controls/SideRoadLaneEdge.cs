using System;
using RoadTrafficSimulator.Components.BuildMode.Connectors;
using RoadTrafficSimulator.Components.BuildMode.VertexContainers;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Draw;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class SideRoadLaneEdge : Edge
    {
        private RoadLaneBlock _parent;
        private readonly SideRoadLaneEdgeVertexContainer _vertexContainer;
        private readonly SideRoadLaneConnector _connector;
        private readonly Routes _routes = new Routes();
        private LaneType _laneType;

        public SideRoadLaneEdge(Factories.Factories factories, MovablePoint startPoint, MovablePoint endPoint, RoadLaneBlock parent)
            : base( factories, startPoint, endPoint, Styles.NormalStyle )
        {
            this._connector = new SideRoadLaneConnector( this );
            this._parent = parent;
            this._laneType = LaneType.SolidLine;
            this._vertexContainer = new SideRoadLaneEdgeVertexContainer( this );
        }

        public Routes Routes { get { return this._routes; } }

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
            set
            {
                if ( ( value is RoadLaneBlock ) == false ) { throw new ArgumentException("Only RoadLaneBlockI is valid"); }
                this._parent = (RoadLaneBlock) value;
            }
        }

        public SideRoadLaneConnector Connector
        {
            get { return this._connector; }
        }
    }
}
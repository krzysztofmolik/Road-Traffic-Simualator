using RoadTrafficSimulator.Components.BuildMode.Connectors;
using RoadTrafficSimulator.Infrastructure.Mouse;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class EndRoadLaneEdge : Edge
    {
        private readonly RoadLaneBlock _parrent;
        private readonly IMouseHandler _notMovableMouseHandler;

        public EndRoadLaneEdge( Factories.Factories factories, RoadLaneBlock parent )
            : base( factories, Styles.NormalStyle, parent )
        {
            this._parrent = parent;
            this._notMovableMouseHandler = factories.MouseHandlerFactory.CreateEmpty();
            this.Connector = new EndRoadLaneEdgeConnector( this );
        }

        public EndRoadLaneEdge( Factories.Factories factories, MovablePoint startPoint, MovablePoint endPoint, RoadLaneBlock parent )
            : base( factories, startPoint, endPoint, Styles.NormalStyle, parent )
        {
            this._parrent = parent;
            this._notMovableMouseHandler = factories.MouseHandlerFactory.CreateEmpty();
            this.Connector = new EndRoadLaneEdgeConnector( this );
        }

        public RoadLaneBlock RoadLaneBlockParent
        {
            get { return this._parrent; }
        }

        public EndRoadLaneEdgeConnector Connector
        {
            get;
            private set;
        }

        public override IMouseHandler MouseHandler
        {
            get { return this._notMovableMouseHandler; }
        }

        public EndRoadLaneEdge GetOppositeEdge()
        {
            if ( this._parrent.LeftEdge == this )
            {
                return this._parrent.RightEdge;
            }

            // else
            return this._parrent.LeftEdge;
        }
    }
}
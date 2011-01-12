using RoadTrafficSimulator.Infrastructure.Control;

namespace RoadTrafficSimulator.Road.Controls
{
    public class EndRoadLaneEdge : Edge
    {
        private readonly RoadLaneBlock _parrent;

        public EndRoadLaneEdge( RoadLaneBlock parent )
        {
            this._parrent = parent;
        }

        public EndRoadLaneEdge( MovablePoint startPoint, MovablePoint endPoint, float width, RoadLaneBlock parent )
            : base( startPoint, endPoint, width )
        {
            this._parrent = parent;
        }

        public RoadLaneBlock RoadLaneBlockParent
        {
            get { return this._parrent; }
        }

        public override IControl Parent
        {
            get { return this._parrent; }
        }
    }
}
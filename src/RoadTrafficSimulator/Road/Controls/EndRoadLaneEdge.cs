using System.Diagnostics;
using System.Linq;
using RoadTrafficSimulator.Road.Connectors;

namespace RoadTrafficSimulator.Road.Controls
{
    public class EndRoadLaneEdge : Edge
    {
        public EndRoadLaneEdge( RoadLaneBlock parent )
            : base( parent )
        {
        }

        public EndRoadLaneEdge( MovablePoint startPoint, MovablePoint endPoint, float width, RoadLaneBlock parent )
            : base( startPoint, endPoint, width, parent )
        {
        }

        public RoadLaneBlock RoadLaneBlockParent
        {
            get
            {
                var result = this.Parents.First() as RoadLaneBlock;
                Debug.Assert( result != null, "result != null" );
                return result;
            }
        }
    }
}
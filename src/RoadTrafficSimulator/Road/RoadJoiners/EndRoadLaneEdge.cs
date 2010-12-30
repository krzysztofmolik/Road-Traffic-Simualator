using System.Diagnostics;
using System.Linq;
using XnaRoadTrafficConstructor.Road.RoadJoiners;

namespace RoadTrafficSimulator.Road.RoadJoiners
{
    public class EndRoadLaneEdge : EdgeBase
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
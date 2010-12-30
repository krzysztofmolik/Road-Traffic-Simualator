using System.Diagnostics;
using System.Linq;
using XnaRoadTrafficConstructor.Road.RoadJoiners;

namespace RoadTrafficSimulator.Road.RoadJoiners
{
    public class RoadJunctionEdge : EdgeBase
    {
        public RoadJunctionEdge( RoadJunctionBlock parent )
            : base( parent )
        {
        }

        public RoadJunctionEdge( MovablePoint startPoint, MovablePoint endPoint, float width, RoadJunctionBlock parent )
            : base( startPoint, endPoint, width, parent )
        {
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
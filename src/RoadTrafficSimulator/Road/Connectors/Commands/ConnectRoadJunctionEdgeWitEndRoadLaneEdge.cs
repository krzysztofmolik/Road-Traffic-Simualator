using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Road.Controls;

namespace RoadTrafficSimulator.Road.Connectors.Commands
{
    public class ConnectRoadJunctionEdgeWitEndRoadLaneEdge : IConnectionCommand
    {
        public virtual bool Connect(ILogicControl first, ILogicControl second)
        {
            var roadJunctionEdge = first as RoadJunctionEdge;
            var roadLaneEdge = second as EndRoadLaneEdge;

            if ( roadLaneEdge == null || roadJunctionEdge == null )
            {
                return false;
            }

            roadJunctionEdge.Connector.ConnectEndWith( roadLaneEdge );
            roadLaneEdge.Connector.ConnectBeginWith(roadJunctionEdge);
            return true;
        }
    }
}
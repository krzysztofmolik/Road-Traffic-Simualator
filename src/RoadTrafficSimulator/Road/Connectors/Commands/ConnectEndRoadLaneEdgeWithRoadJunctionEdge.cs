using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Road.Controls;

namespace RoadTrafficSimulator.Road.Connectors.Commands
{
    public class ConnectEndRoadLaneEdgeWithRoadJunctionEdge : IConnectionCommand
    {
        public virtual bool Connect(ILogicControl first, ILogicControl second)
        {
            var roadLaneEdge = first as EndRoadLaneEdge;
            var roadJunctionEdge = second as RoadJunctionEdge;

            if ( roadLaneEdge == null || roadJunctionEdge == null )
            {
                return false;
            }

            roadLaneEdge.Connector.ConnectEndWith(roadJunctionEdge);
            roadJunctionEdge.Connector.ConnectBeginWith( roadLaneEdge );

            return true;
        }
    }
}
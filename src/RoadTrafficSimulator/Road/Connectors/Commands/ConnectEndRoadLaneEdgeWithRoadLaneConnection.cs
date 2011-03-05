using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Road.Controls;

namespace RoadTrafficSimulator.Road.Connectors.Commands
{
    public class ConnectEndRoadLaneEdgeWithRoadLaneConnection : IConnectionCommand
    {
        public virtual bool Connect( ILogicControl first, ILogicControl second )
        {
            var roadLaneEdge = first as EndRoadLaneEdge;
            var roadConnection = second as RoadConnection;
            if ( roadLaneEdge == null || roadConnection == null )
            {
                return false;
            }

            roadLaneEdge.Connector.ConnectEndWith( roadConnection );
            roadConnection.Connector.ConnectBeginWith( roadLaneEdge );
            return true;
        }
    }
}
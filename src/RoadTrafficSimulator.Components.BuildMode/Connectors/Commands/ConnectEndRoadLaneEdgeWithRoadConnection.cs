using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Connectors.Commands
{
    public class ConnectEndRoadLaneEdgeWithRoadConnection : IConnectionCommand
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
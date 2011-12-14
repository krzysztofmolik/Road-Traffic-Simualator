using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Connectors.Commands
{
    public class ConnectRoadLaneWithRoadConnection : IConnectionCommand
    {
        public virtual bool Connect( ILogicControl first, ILogicControl second )
        {
            var roadLaneEdge = first as RoadLaneBlock;
            var roadConnection = second as RoadConnection;
            if ( roadLaneEdge == null || roadConnection == null )
            {
                return false;
            }

            roadLaneEdge.RightEdge.Connector.ConnectStartFrom( roadConnection );
            roadConnection.Connector.ConnectBeginWith( roadLaneEdge.RightEdge );
            return true;
        }
    }
}
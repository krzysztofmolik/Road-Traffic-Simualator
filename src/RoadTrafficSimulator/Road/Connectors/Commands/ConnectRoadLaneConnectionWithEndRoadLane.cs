using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Road.Controls;

namespace RoadTrafficSimulator.Road.Connectors.Commands
{
    public class ConnectRoadLaneConnectionWithEndRoadLane : IConnectionCommand
    {
        public virtual bool Connect(ILogicControl first, ILogicControl second)
        {
            var roadConnectionEdge = first as RoadConnection;
            var roadLaneEdge = second as EndRoadLaneEdge;
            if ( roadConnectionEdge == null || roadLaneEdge == null )
            {
                return false;
            }

            // TODO Check it
            roadConnectionEdge.Connector.ConnectEndWith( roadLaneEdge );
            roadLaneEdge.Connector.ConnectBeginWith( roadConnectionEdge );

            return true;
        }
    }
}
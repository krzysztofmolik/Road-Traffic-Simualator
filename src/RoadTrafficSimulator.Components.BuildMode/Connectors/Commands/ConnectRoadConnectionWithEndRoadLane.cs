using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Connectors.Commands
{
    public class ConnectRoadConnectionWithEndRoadLane : IConnectionCommand
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
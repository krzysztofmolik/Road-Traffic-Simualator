using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Connectors.Commands
{
    public class ConnectRoadConnectionWithRoadLane : IConnectionCommand
    {
        public virtual bool Connect( ILogicControl first, ILogicControl second )
        {
            var roadConnectionEdge = first as RoadConnection;
            var roadLaneEdge = second as RoadLaneBlock;
            if ( roadConnectionEdge == null || roadLaneEdge == null )
            {
                return false;
            }

            // TODO Check it
            roadConnectionEdge.Connector.ConnectEndWith( roadLaneEdge.LeftEdge );
            roadLaneEdge.LeftEdge.Connector.ConnectEndOn( roadConnectionEdge );

            return true;
        }
    }
}
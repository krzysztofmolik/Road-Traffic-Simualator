using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Road.Controls;

namespace RoadTrafficSimulator.Road.Connectors.Commands
{
    public class ConnectRoadLaneConnectionWithEndRoadLane : IConnectionCommand
    {
        public virtual bool Connect( IControl first, IControl second )
        {
            var roadConnectionEdge = first as RoadConnectionEdge;
            var roadLaneEdge = second as EndRoadLaneEdge;
            if ( roadConnectionEdge == null || roadLaneEdge == null )
            {
                return false;
            }

            roadConnectionEdge.Connector.ConnectWith( roadLaneEdge );

            return true;
        }
    }
}
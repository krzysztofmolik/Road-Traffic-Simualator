using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Road.Controls;

namespace RoadTrafficSimulator.Road.Connectors.Commands
{
    public class ConnectEndRoadLaneEdgeWithRoadLaneConnection : IConnectionCommand
    {
        public virtual bool Connect( IControl first, IControl second )
        {
            var roadLaneEdge = first as EndRoadLaneEdge;
            var roadConnection = second as RoadConnectionEdge;
            if ( roadLaneEdge == null || roadConnection == null )
            {
                return false;
            }

            roadConnection.Connector.ConnectTo( roadLaneEdge );
            return true;
        }
    }
}
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Road.Controls;

namespace RoadTrafficSimulator.Road.Connectors.Commands
{
    public class ConnectEndRoadLaneEdgeWithRoadJunctionEdge : IConnectionCommand
    {
        public virtual bool Connect( IControl first, IControl second )
        {
            var roadLaneEdge = first as EndRoadLaneEdge;
            var roadJunctionEdge = second as RoadJunctionEdge;

            if ( roadLaneEdge == null || roadJunctionEdge == null )
            {
                return false;
            }

            roadJunctionEdge.Connector.ConnectTo( roadLaneEdge );
            return true;
        }
    }
}
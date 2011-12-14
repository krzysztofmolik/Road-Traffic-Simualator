using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Connectors.Commands
{
    public class ConnectRoadLaneWithJunctionEdge : IConnectionCommand
    {
        public virtual bool Connect(ILogicControl first, ILogicControl second)
        {
            var roadLaneEdge = first as RoadLaneBlock;
            var roadJunctionEdge = second as JunctionEdge;

            if ( roadLaneEdge == null || roadJunctionEdge == null )
            {
                return false;
            }

            roadLaneEdge.RightEdge.Connector.ConnectStartFrom(roadJunctionEdge);
            roadJunctionEdge.Connector.ConnectEndsOn( roadLaneEdge );

            return true;
        }
    }
}
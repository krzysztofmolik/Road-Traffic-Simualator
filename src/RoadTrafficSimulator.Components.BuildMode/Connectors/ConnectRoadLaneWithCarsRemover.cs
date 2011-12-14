using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Connectors
{
    public class ConnectRoadLaneWithCarsRemover : IConnectionCommand
    {
        public bool Connect(ILogicControl first, ILogicControl second)
        {
            var roadLaneEdge = first as RoadLaneBlock;
            var carsRemover = second as CarsRemover;

            if ( roadLaneEdge == null || carsRemover == null ) { return false; }

            roadLaneEdge.RightEdge.Connector.ConnectStartFrom( carsRemover );
            carsRemover.Connector.ConnectEndOn( roadLaneEdge );

            return true;
        }
    }
}
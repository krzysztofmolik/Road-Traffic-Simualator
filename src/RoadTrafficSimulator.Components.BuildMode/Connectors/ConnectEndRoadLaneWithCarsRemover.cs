using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Connectors
{
    public class ConnectEndRoadLaneWithCarsRemover : IConnectionCommand
    {
        public bool Connect(ILogicControl first, ILogicControl second)
        {
            var roadLaneEdge = first as EndRoadLaneEdge;
            var carsRemover = second as CarsRemover;

            if ( roadLaneEdge == null || carsRemover == null ) { return false; }

            roadLaneEdge.Connector.ConnectEndWith( carsRemover );
            carsRemover.Connector.ConnectBeginWith( roadLaneEdge );

            return true;
        }
    }
}
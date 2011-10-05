using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Connectors
{
    public class ConnectCarInserterWithEndRoadLane : IConnectionCommand
    {
        public bool Connect( ILogicControl first, ILogicControl second )
        {
            var carsInseerter = first as CarsInserter;
            var endRoadLaneEdge = second as EndRoadLaneEdge;

            if ( carsInseerter == null || endRoadLaneEdge == null ) { return false; }

            carsInseerter.Connector.ConnectEndWith( endRoadLaneEdge );
            endRoadLaneEdge.Connector.ConnectBegintWith( carsInseerter );

            return true;
        }
    }
}
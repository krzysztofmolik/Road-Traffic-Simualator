using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Road.Controls;

namespace RoadTrafficSimulator
{
    public class ConnectCarInserterWithEnd : IConnectionCommand
    {
        public bool Connect( ILogicControl first, ILogicControl second )
        {
            var carsInseerter = first as CarsInserter;
            var endRoadLaneEdge = second as EndRoadLaneEdge;

            if ( carsInseerter == null || endRoadLaneEdge == null ) { return false; }

            carsInseerter.Connector.ConnectBeginWith( endRoadLaneEdge );
            endRoadLaneEdge.Connector.ConnectEndWith(carsInseerter.RightEdge);

            return true;
        }
    }
}
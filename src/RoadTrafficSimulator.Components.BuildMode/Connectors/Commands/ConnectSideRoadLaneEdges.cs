using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Connectors.Commands
{
    public class ConnectSideRoadLaneEdges : IConnectionCommand
    {
        public bool Connect(ILogicControl first, ILogicControl second)
        {
            var firstEdge = first as SideRoadLaneEdge;
            if ( firstEdge == null )
            {
                return false;
            }

            var secondEdge = second as SideRoadLaneEdge;
            if ( secondEdge == null )
            {
                return false;
            }

            if ( this.AreConnectedInTheSamePlaces( firstEdge, secondEdge ) == false )
            {
                return false;
            }
            
            firstEdge.Connector.ConnectChangeName( secondEdge );
            secondEdge.Connector.ConnectChangeName( firstEdge );
            return true;
        }

        private bool AreConnectedInTheSamePlaces(SideRoadLaneEdge firstEdge, SideRoadLaneEdge secondEdge)
        {
            var theSameDirection =
                (firstEdge.StartLocation == secondEdge.StartLocation) && 
                (firstEdge.EndLocation == secondEdge.EndLocation);
            var oppositeDirection = 
                (firstEdge.StartLocation == secondEdge.EndLocation) && 
                (firstEdge.EndLocation == secondEdge.StartLocation);

            return theSameDirection || oppositeDirection;
        }
    }
}
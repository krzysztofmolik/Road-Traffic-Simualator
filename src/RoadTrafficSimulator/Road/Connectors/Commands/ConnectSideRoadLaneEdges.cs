using System;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Road.Controls;

namespace RoadTrafficSimulator.Road.Connectors.Commands
{
    public class ConnectSideRoadLaneEdges : IConnectionCommand
    {
        public bool Connect(IControl first, IControl second)
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
            
            firstEdge.Connector.ConnectTo( secondEdge );
            secondEdge.Connector.ConnectTo( firstEdge );
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
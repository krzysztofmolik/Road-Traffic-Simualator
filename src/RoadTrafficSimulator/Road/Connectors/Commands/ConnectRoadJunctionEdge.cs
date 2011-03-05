using System;
using System.Linq;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Road.Controls;

namespace RoadTrafficSimulator.Road.Connectors.Commands
{
    public class ConnectRoadJunctionEdge : IConnectionCommand
    {
        public bool Connect(ILogicControl first, ILogicControl second)
        {
            var firstEdge = first as RoadJunctionEdge;
            var secondEdge = second as RoadJunctionEdge;
            if ( firstEdge == null || secondEdge == null )
            {
                return false;
            }

            if ( this.HaveTheSameParent( firstEdge, secondEdge ) )
            {
                return false;
            }

            if ( this.AreConnected( firstEdge, secondEdge ) )
            {
                return false;
            }

            firstEdge.Connector.ConnectEndWith( secondEdge );
            secondEdge.Connector.ConnectBeginWith( firstEdge );

            firstEdge.RecalculatePosition();
            secondEdge.RecalculatePosition();
            return true;
        }

        private bool HaveTheSameParent(RoadJunctionEdge first, RoadJunctionEdge second)
        {
            var firstParent = first.Parent as ICompositeControl;
            if ( firstParent == null )
            {
                return false;
            }

            var theSameParent = firstParent.Children.Any( c => c == second );
            return theSameParent;
        }

        private bool AreConnected( RoadJunctionEdge firstEdge, RoadJunctionEdge secondEdge )
        {
            return firstEdge.Connector.AreConnected(secondEdge);
        }
    }
}
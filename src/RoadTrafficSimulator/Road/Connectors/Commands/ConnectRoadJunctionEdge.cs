using System.Linq;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Road.Controls;

namespace RoadTrafficSimulator.Road.Connectors.Commands
{
    public class ConnectRoadJunctionEdge : IConnectionCommand
    {
        public bool Connect( IControl first, IControl second )
        {
            var firstEdge = first as RoadJunctionEdge;
            var secondEdge = second as RoadJunctionEdge;
            if ( firstEdge == null || secondEdge == null )
            {
                return false;
            }

            if ( this.AreConnected( firstEdge, secondEdge ) )
            {
                return false;
            }

            firstEdge.Connector.ConnectWith( secondEdge );
            secondEdge.Connector.ConnectTo( firstEdge );
            return true;
        }

        private bool AreConnected( RoadJunctionEdge firstEdge, RoadJunctionEdge secondEdge )
        {
            var firstParent = firstEdge.Parent as ICompositeControl;
            if ( firstParent == null )
            {
                return false;
            }

            var theSameParent = firstParent.Children.Any( c => c == secondEdge );
            return theSameParent == false;
        }
    }
}
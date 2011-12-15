using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Connectors.Commands
{
    public class ConnectRoadJunctionEdge : IConnectionCommand
    {
        public bool Connect( ILogicControl first, ILogicControl second )
        {
            var firstEdge = first as JunctionEdge;
            var secondEdge = second as JunctionEdge;
            if ( firstEdge == null || secondEdge == null )
            {
                return false;
            }

            if ( this.AreConnected( firstEdge, secondEdge ) )
            {
                return false;
            }

            firstEdge.Connector.ConnectBeginFrom( secondEdge );
            secondEdge.Connector.ConnectEndsOn( firstEdge );

            return true;
        }


        private bool AreConnected( JunctionEdge first, JunctionEdge second )
        {
            if ( first.Connector.Edge != null && first.Connector.Edge.Parent == second ) { return true; }
            if ( second.Connector.Edge != null && second.Connector.Edge == first ) { return true; }
            return false;
        }
    }
}
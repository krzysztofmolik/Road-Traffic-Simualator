using System;
using System.Linq;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Route;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors.LaneJunctionConductor
{
    public class LaneJucntionConductorJunctionInformation
    {
        private readonly LaneJunction _laneJunction;

        public LaneJucntionConductorJunctionInformation( LaneJunction laneJunction )
        {
            this._laneJunction = laneJunction;
        }

        public void GetNextJunctionInformation( RouteMark route, JunctionInformation junctionInformation )
        {
            var inEdge = this.GetEdgeConnectedWith( route.GetPrevious() );
            var outEdge = this.GetEdgeConnectedWith( route.GetNext() );
            if ( inEdge.Situation.OnTheRight == outEdge )
            {
                this.TurnRight( inEdge, outEdge, junctionInformation, route );
            }
            else if ( inEdge.Item2.Situation.OnTheFront == outEdge.Item2 )
            {
                this.Straight( inEdge, outEdge, junctionInformation, route );
            }
            else if ( inEdge.Item2.Situation.OnTheLeft == outEdge.Item2 )
            {
                this.TurnLeft( inEdge, outEdge, junctionInformation, route );
            }
            else
            {
                throw new InvalidOperationException( "Invalid edge" );
            }
        }

        private JunctionEdge GetEdgeConnectedWith( IRoadElement roadElement )
        {
            var item = this._laneJunction.Edges.Where( s => s.Lane == roadElement ).FirstOrDefault();
            return item;
        }
    }
}
using System;
using System.Collections.Generic;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors.Infrastructure;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;
using System.Linq;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors
{
    [ConductorSupportedRoadElementType( typeof( LaneJunction ) )]
    [PriorityConductorInformation( PriorityType.FromRight )]
    public class FromRightJunctionConductor : JunctionConductorBase
    {
        private enum SideMove
        {
            OnTheLeft = 1,
            OnFront = 2,
            OnTheRight = 3,
        }

        // BUG This can be calculated once on every 200 ms
        private IEnumerable<PriorityInformation> GetPriorityCarInfromation( IRouteMark<IConductor> route, SideMove sideMove )
        {
            var onTheRight = this.GetJunctionEdgeOnTheRigh( route, sideMove );
            // NOTE I can do this, because I am sure that this Junction edge and it has to be connected with junction!!
            var resversIterator = new RouteJunctionReversIterator( this.Junction, onTheRight.Routes.BelongToRoutes, UnitConverter.FromMeter( 50 ) );

#if DEBUG
            var tes = resversIterator.ToArray();
#endif

            return resversIterator.Select( r =>
                                               {
                                                   var firstCarToOut = r.RoadElement.Information.GetFirstCarToOutInformation();
                                                   return new
                                                              {
                                                                  firstCarToOut.Car,
                                                                  firstCarToOut.CarDistance,
                                                                  r.Distance
                                                              };
                                               } )



                .Where( c => c.Car != null )
                .Where( c => this.DriverThruJunction( c.Car ) )
                .Select( c => new PriorityInformation( c.Car, c.CarDistance, c.Distance ) );
        }

        private JunctionEdge GetJunctionEdgeOnTheRigh( IRouteMark<IConductor> route, SideMove sideMOve )
        {
            var previous = route.GetPrevious();
            var ed = this.Junction.JunctionBuilder.Connector.Edges.Select( ( e, i ) => new
                                                                                  {
                                                                                      Index = i,
                                                                                      Element = e
                                                                                  } )
                .First( e => e.Element == previous.RouteElement.RoadElement.BuildControl );

            var resultIndex = ( int ) ( ed.Index + sideMOve ) % 4;

            switch ( resultIndex )
            {
                case EdgeType.Bottom:
                    return this.Junction.Bottom;
                case EdgeType.Top:
                    return this.Junction.Top;
                case EdgeType.Left:
                    return this.Junction.Left;
                case EdgeType.Right:
                    return this.Junction.Right;
            }

            throw new ArgumentException();
        }

        private bool DriverThruJunction( Car firtCarToOut )
        {
            var condcutors = firtCarToOut.Conductors.Clone();
            while ( condcutors.MoveNext() )
            {
                if ( condcutors.Current.RouteElement.RoadElement == this.Junction )
                {
                    return true;
                }
            }
            return false;
        }

        protected override PriorityInformation[] GetPriorityCarInfromation( Car car, IRouteMark<IConductor> route )
        {
            if ( this.PriorityTypes == PriorityType.None || this.PriorityTypes == PriorityType.Light ) { return new PriorityInformation[ 0 ]; }
            return this.GetPriorityCarInfromation( route, this.GetMoveSide( this.PriorityTypes ) ).ToArray();
        }

        private SideMove GetMoveSide( PriorityType priorityType )
        {
            switch ( priorityType )
            {
                case PriorityType.FromFront:
                    return SideMove.OnFront;
                case PriorityType.FromRight:
                    return SideMove.OnTheRight;
                case PriorityType.FromLeft:
                    return SideMove.OnTheLeft;
            }

            throw new ArgumentException();
        }
    }

    public struct RouteElementWithDistance
    {
        public RouteElementWithDistance( IRoadElement roadElement, float distance )
            : this()
        {
            this.RoadElement = roadElement;
            this.Distance = distance;
        }

        public IRoadElement RoadElement { get; private set; }
        public float Distance { get; private set; }
    }
}
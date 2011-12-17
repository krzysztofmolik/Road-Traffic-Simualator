using System;
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
        // BUG This can be calculated once on every 200 ms
        protected override PriorityInformation[] GetPriorityCarInfromation( Car car, IRouteMark<IConductor> route )
        {
            var onTheRight = this.GetJunctionEdgeOnTheRigh( route );
            // NOTE I can do this, because I'am sure that this Junction edge and it has to be connected with junction!!
            var resversIterator = new RouteJunctionReversIterator( onTheRight, UnitConverter.FromMeter( 50 ) );

            var tes = resversIterator.ToArray();

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
                .Select( c => new PriorityInformation( c.Car, c.CarDistance, c.Distance ) )
                .ToArray();
        }

        private JunctionEdge GetJunctionEdgeOnTheRigh( IRouteMark<IConductor> route )
        {
            var previous = route.GetPrevious();
            if ( this.Junction.JunctionBuilder.Connector.LeftEdge == previous.RouteElement.RoadElement.BuildControl )
            {
                return this.Junction.Bottom;
            }
            if ( this.Junction.JunctionBuilder.Connector.TopEdge == previous.RouteElement.RoadElement.BuildControl )
            {
                return this.Junction.Left;
            }
            if ( this.Junction.JunctionBuilder.Connector.RightEdge == previous.RouteElement.RoadElement.BuildControl )
            {
                return this.Junction.Top;
            }
            if ( this.Junction.JunctionBuilder.Connector.BottomEdge == previous.RouteElement.RoadElement.BuildControl )
            {
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
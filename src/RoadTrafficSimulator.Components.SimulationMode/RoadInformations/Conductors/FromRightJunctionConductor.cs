using System.Collections;
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
        private readonly LaneJunction _laneJunction;

        public FromRightJunctionConductor( LaneJunction laneJunction )
        {
            this._laneJunction = laneJunction;
        }

        // BUG This can be calculated once on every 200 ms
        protected override PriorityInformation[] GetPriorityCarInfromation( Car car, IRouteMark<IConductor> route )
        {
            var resversIterator = new ReversRouteIterator( route.Current.RoadElement.RoadInformation,
                                                           UnitConverter.FromMeter( 50 ) );

            return resversIterator.Select( r =>
                                               {
                                                   var firstCarToOut = r.RoadInformation.GetFirstCarToOutInformation();
                                                   return new
                                                              {
                                                                  FirtCarToOut = firstCarToOut.Car,
                                                                  CarDistanceToJunction = firstCarToOut.CarDistance,
                                                                  Distance = r.Distance
                                                              };
                                               } )



                .Where( c => c.FirtCarToOut != null )
                .Where( c => this.DriverThruJunction( c.FirtCarToOut ) )
                .Select( c => new PriorityInformation( c.FirtCarToOut, c.CarDistanceToJunction, c.Distance ) )
                .ToArray();
        }
    }

    public struct RoadElementWithDistance
    {
        public RoadElementWithDistance( IRoadElement roadElement, float distance )
            : this()
        {
            this.RoadElement = roadElement;
            this.Distance = distance;
        }

        public IRoadElement RoadElement { get; private set; }
        public float Distance { get; private set; }
    }

    public class ReversRouteIterator : IEnumerable<RoadElementWithDistance>
    {
        private readonly IRoadInformation _orginal;
        private readonly float _maxDistance;
        private readonly List<IRoadElement> _visited = new List<IRoadElement>();

        public ReversRouteIterator( IRoadInformation orginal, float maxDistance )
        {
            this._orginal = orginal;
            this._maxDistance = maxDistance;
        }

        // TODO Przekazywac dlugosc
        public IEnumerator<RoadElementWithDistance> GetEnumerator()
        {
            return this._orginal.ReversConnection.SelectMany( e => this.GetElemnts( e, 0.0f ) ).GetEnumerator();
        }

        private IEnumerable<RoadElementWithDistance> GetElemnts( IRoadElement roadElement, float distance )
        {
            if ( this._visited.Contains( roadElement ) ) { return Enumerable.Empty<RoadElementWithDistance>(); }

            this._visited.Add( roadElement );

    var newDistance = distance + roadElement.RoadInformation.Lenght(  )
            return new[] { new RoadElementWithDistance( roadElement, distance ) }.Concat( roadElement.RoadInformation.ReversConnection.SelectMany( this.GetElemnts() ) );

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
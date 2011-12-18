using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Builder;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors.Infrastructure;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors
{
    [ConductorSupportedRoadElementType( typeof( JunctionEdge ) )]
    [PriorityConductorInformation( PriorityType.FromRight )]
    [PriorityConductorInformation( PriorityType.None )]
    [PriorityConductorInformation( PriorityType.FromLeft )]
    [PriorityConductorInformation( PriorityType.FromFront )]
    public class SimpleJunctionEdgeConductor : IConductor
    {
        private JunctionEdge _junctionEdge;
        private bool _canStopOnIt;

        public RoadInformation Process( Car car, IRouteMark<IConductor> route )
        {
            var carAheadInformation = this.Information.GetCarAheadDistance( car );
            return new RoadInformation
                       {
                           CarAhead = carAheadInformation.CarAhead,
                           CarAheadDistance = carAheadInformation.CarDistance,
                           PrivilagesCarInformation = null,
                       };
        }

        public void SetRouteElement( IRoadElement element )
        {
            var junctionEdge = element as JunctionEdge;
            if ( junctionEdge == null ) { throw new ArgumentException( "Wrong road element" ); }
            this._junctionEdge = junctionEdge;
        }

        public IRoadInformation Information { get { return this._junctionEdge.Information; } }
        public RouteElement RouteElement { get; private set; }

        public void Setup( RouteElement roadElement, bool canStopOnIt, IRoadElement previous, IRoadElement next, PriorityType priorityType )
        {
            this.SetRouteElement( roadElement.RoadElement );
            this._canStopOnIt = canStopOnIt;
            this.RouteElement = roadElement;
        }

        public Vector2 GetCarDirection( Car car )
        {
            return car.Direction;
        }

        public float GetCarDistanceToEnd( Car car )
        {
            Debug.Assert( this.Information.ContainsCar( car ) );
            return Constans.PointSize;
        }

        public IRoadElement RoadElement
        {
            get { return this._junctionEdge; }
        }
    }
}
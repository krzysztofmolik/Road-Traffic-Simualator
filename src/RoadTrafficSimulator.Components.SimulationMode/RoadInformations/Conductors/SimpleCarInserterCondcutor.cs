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
    [ConductorSupportedRoadElementType( typeof( CarsInserter ) )]
    [PriorityConductorInformation( PriorityType.None )]
    public class SimpleCarInserterCondcutor : IConductor
    {
        private bool _canStopOnIt;
        private CarsInserter _carInserter;
        private RouteElement _routeElement;
        private IRoadElement _next;

        public RoadInformation Process( Car car, IRouteMark<IConductor> route )
        {
            var carAheadInformation = this.Information.GetCarAheadDistance( car );
            return new RoadInformation
                       {
                           CarAhead = carAheadInformation.CarAhead,
                           CarAheadDistance = carAheadInformation.CarDistance,
                           PrivilagesCarInformation = null,
                           CanStop = this._canStopOnIt,
                       };
        }

        private void SetRouteElement( IRoadElement element )
        {
            var carInserter = element as CarsInserter;
            if ( carInserter == null ) { throw new ArgumentException( "Wrong road element" ); }
            this._carInserter = carInserter;
        }

        public IRoadInformation Information
        {
            get { return this._carInserter.Information; }
        }

        public RouteElement RouteElement
        {
            get { return this._routeElement; }
        }

        public void Setup( RouteElement roadElement, bool canStopOnIt, IRoadElement previous, IRoadElement next, PriorityType priorityType )
        {
            this.SetRouteElement( roadElement.RoadElement );
            this._canStopOnIt = canStopOnIt;
            this._routeElement = roadElement;
            this._next = next;
        }

        public Vector2 GetCarDirection( Car car )
        {
            return this._next.BuildControl.Location - car.Location;
        }

        public float GetCarDistanceToEnd( Car car )
        {
            Debug.Assert( this.Information.ContainsCar( car ) );
            return Constans.PointSize;
        }
    }
}
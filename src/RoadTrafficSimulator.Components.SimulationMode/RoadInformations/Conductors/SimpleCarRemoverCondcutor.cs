using System;
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
    [ConductorSupportedRoadElementType( typeof( CarsRemover ) )]
    [PriorityConductorInformation( PriorityType.None )]
    public class SimpleCarRemoverCondcutor : IConductor
    {
        private CarsRemover _carRemover;
        private bool _canStopOnIt;
        private RouteElement _routeElement;

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

        private void SetRouteElement( IRoadElement element )
        {
            var carRemover = element as CarsRemover;
            if ( carRemover == null ) { throw new ArgumentException( "Wrong road element" ); }
            this._carRemover = carRemover;
        }

        public IRoadInformation Information
        {
            get { return this._carRemover.Information; }
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
        }

        public Vector2 GetCarDirection( Car car )
        {
            return car.Direction;
        }

        public float GetCarDistanceToEnd( Car car )
        {
            return Constans.PointSize;
        }

        public IRoadElement RoadElement
        {
            get { return this._carRemover; }
        }
    }
}
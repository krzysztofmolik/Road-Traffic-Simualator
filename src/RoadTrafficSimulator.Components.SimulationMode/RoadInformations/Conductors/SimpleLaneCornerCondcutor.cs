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
    [ConductorSupportedRoadElementType( typeof( LaneCorner ) )]
    [PriorityConductorInformation( PriorityType.None )]
    public class SimpleLaneCornerCondcutor : IConductor
    {
        private LaneCorner _laneCorner;
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

        private void SetRouteElement( IRoadElement element )
        {
            var laneCorner = element as LaneCorner;
            if ( laneCorner == null ) { throw new ArgumentException( "Wrong road element" ); }
            this._laneCorner = laneCorner;
        }

        public IRoadInformation Information
        {
            get { return this._laneCorner.Information; }
        }

        public RouteElement RouteElement { get; private set; }

        public void Setup( RouteElement roadElement, bool canStopOnIt, IRoadElement previous, IRoadElement next )
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
    }
}
using System;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Builder;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors.Infrastructure;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors
{
    [ConductorSupportedRoadElementType( typeof( Lane ) )]
    [PriorityConductorInformation( PriorityType.None )]
    public class SimpleLaneCondcutor : IConductor
    {
        private Lane _lane;
        private bool _canStopOnIt;

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
            var lane = element as Lane;
            if ( lane == null ) { throw new ArgumentException( "Wrong road element" ); }
            this._lane = lane;
        }

        public IRoadInformation Information
        {
            get { return this._lane.Information; }
        }

        public RouteElement RouteElement { get; private set; }

        public void Setup( RouteElement roadElement, bool canStopOnIt, IRoadElement previous, IRoadElement next, PriorityType priorityType )
        {
            this.SetRouteElement( roadElement.RoadElement );
            this._canStopOnIt = canStopOnIt;
            this.RouteElement = roadElement;
        }

        public Vector2 GetCarDirection( Car car )
        {
            return this._lane.RoadLaneBlock.RightEdge.Location - car.Location;
        }

        public float GetCarDistanceToEnd( Car car )
        {
            return Vector2.Distance( car.Location, this._lane.RoadLaneBlock.RightEdge.Location );
        }

        public IRoadElement RoadElement
        {
            get { return this._lane; }
        }
    }
}
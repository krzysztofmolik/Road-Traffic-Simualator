using System;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Builder;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using RoadTrafficSimulator.Infrastructure.Controls;
using System.Linq;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors
{
    public abstract class JunctionConductorBase : IConductor
    {
        private bool _canStopOnIt;
        private RouteElement _routeElement;
        private IRoadElement _previous;
        private IRoadElement _next;

        public RoadInformation Process( Car car, IRouteMark<IConductor> route )
        {
            var carAheadInformation = this.Information.GetCarAheadDistance( car );
            // TODO This should be changed, GetCarAheadDistance can be moved to IConductor
            if ( carAheadInformation.CarAhead != null )
            {
                carAheadInformation.CarDistance += Vector2.Distance( this._previous.BuildControl.Location, carAheadInformation.CarAhead.Location );
            }
            return new RoadInformation
                       {
                           CarAhead = carAheadInformation.CarAhead,
                           CarAheadDistance = carAheadInformation.CarDistance,
                           PrivilagesCarInformation = this.GetPriorityCarInfromation( car, route ),
                           CanStop = this._canStopOnIt,
                       };
        }

        protected abstract PriorityInformation[] GetPriorityCarInfromation( Car car, IRouteMark<IConductor> route );

        protected LaneJunction Junction { get; private set; }

        public IRoadInformation Information
        {
            // TODO Information
            get { return this.Junction.Information; }
        }

        public RouteElement RouteElement
        {
            get { return this._routeElement; }
        }

        public void Setup( RouteElement roadElement, bool canStopOnIt, IRoadElement previous, IRoadElement next, PriorityType priorityType )
        {
            this.Junction = roadElement.RoadElement as LaneJunction;
            if ( this.Junction == null ) { throw new ArgumentException( "Wrong road element" ); }
            this._routeElement = roadElement;
            this._canStopOnIt = canStopOnIt;
            this._previous = previous;
            this._next = next;

            var values = Enum.GetValues( typeof( PriorityType ) );
            this.PriorityTypes = values.Cast<PriorityType>().Where( e => priorityType.HasFlag( e ) ).Where( e => e != PriorityType.None ).ToArray();
        }

        protected PriorityType[] PriorityTypes { get; private set; }

        public Vector2 GetCarDirection( Car car )
        {
            return this._next.BuildControl.Location - car.Location;
        }

        public float GetCarDistanceToEnd( Car car )
        {
            return Vector2.Distance( car.Location, this._next.BuildControl.Location );
        }
    }
}
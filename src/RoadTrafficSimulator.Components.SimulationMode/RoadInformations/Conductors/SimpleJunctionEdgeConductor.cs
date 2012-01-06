using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Builder;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Light;
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
                           CanStop = this._canStopOnIt,
                           CanDriver = this.CheckLights( car ),
                       };
        }

        private bool CheckLights( Car car )
        {
            if ( this._junctionEdge.Light != null )
            {
                // TODO Use switch
                if ( this._junctionEdge.Light.LightState == LightState.Green ) { return true; }
                if ( this._junctionEdge.Light.LightState == LightState.Red ) { return false; }
                if ( this._junctionEdge.Light.LightState == LightState.YiellowFromRed ) { return false; }
                if ( this._junctionEdge.Light.LightState == LightState.YiellowFromGreen )
                {
                    var carDistance = Vector2.Distance( car.Location, this._junctionEdge.EdgeBuilder.Location );
                    return carDistance < UnitConverter.FromMeter( 5 ) && car.Velocity > UnitConverter.FromKmPerHour( 30 );
                }
                throw new ArgumentException( "Not supported ligth state" );
            }
            return true;
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
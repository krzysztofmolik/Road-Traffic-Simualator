using System;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors.Infrastructure;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors
{
    [ConductorSupportedRoadElementType( typeof( Lane ) )]
    [PriorityConductorInformation( PriorityType.None )]
    public class SimpleLanerCondcutor : IConductor
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
                       };
        }

        public void SetRouteElement( IRoadElement element )
        {
            var lane = element as Lane;
            if ( lane == null ) { throw new ArgumentException( "Wrong road element" ); }
            this._lane = lane;
        }

        public void SetCanStopOnIt( bool canStopOnIt )
        {
            this._canStopOnIt = canStopOnIt;
        }

        public IRoadInformation Information
        {
            get { return this._lane.RoadInformation; }
        }

        public IRoadElement RoadElement
        {
            get { return this._lane; }
        }
    }
}
using System;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors.Infrastructure;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors
{
    [ConductorSupportedRoadElementType( typeof( LaneJunction ) )]
    [PriorityConductorInformation( PriorityType.FromRight )]
    [PriorityConductorInformation( PriorityType.None )]
    [PriorityConductorInformation( PriorityType.FromLeft )]
    [PriorityConductorInformation( PriorityType.FromFront )]
    public class SimpleJunctionConductor : IConductor
    {
        private LaneJunction _junction;
        private bool _canStopOnIt;

        public RoadInformation Process( Car car, IRouteMark<IConductor> route )
        {
            return RoadInformation.Empty;
        }

        public void SetRouteElement( IRoadElement element )
        {
            var junction = element as LaneJunction;
            if ( junction == null ) { throw new ArgumentException( "Wrong road element" ); }
            this._junction = junction;
        }

        public void SetCanStopOnIt( bool canStopOnIt )
        {
            this._canStopOnIt = canStopOnIt;
        }

        public IRoadInformation Information
        {
            get { return this._junction.RoadInformation; }
        }

        public IRoadElement RoadElement
        {
            get { return this._junction; }
        }
    }
}
using System;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors.Infrastructure;
using RoadTrafficSimulator.Components.SimulationMode.Route;
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
            return RoadInformation.Empty;
        }

        public void SetRouteElement( IRoadElement element )
        {
            var junctionEdge = element as JunctionEdge;
            if ( junctionEdge == null ) { throw new ArgumentException( "Wrong road element" ); }
            this._junctionEdge = junctionEdge;
        }

        public void SetCanStopOnIt( bool canStopOnIt )
        {
            this._canStopOnIt = canStopOnIt;
        }

        public IRoadInformation Information
        {
            get { return this._junctionEdge.Junction.RoadInformation; }
        }

        public IRoadElement RoadElement
        {
            get { return this._junctionEdge; }
        }
    }
}
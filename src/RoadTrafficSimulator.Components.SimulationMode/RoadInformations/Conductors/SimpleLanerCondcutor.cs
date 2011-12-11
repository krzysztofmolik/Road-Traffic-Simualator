using System;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors
{
    [ConductorSupportedRoadElementType( typeof( Lane ) )]
    [PriorityConductorInformation( PriorityType.None )]
    public class SimpleLanerCondcutor : IConductor
    {
        private Lane _lane;
        private bool _canStopOnIt;

        public bool Process( Car car, RoadInformation endDistance )
        {
            throw new System.NotImplementedException();
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

        public IRoadInformation RoadInformation
        {
            get { return this._lane.RoadInformation; }
        }

        public IRoadElement RoadElement
        {
            get { return this._lane; }
        }
    }
}
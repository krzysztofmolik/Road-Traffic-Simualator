using System;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors
{
    [ConductorSupportedRoadElementType( typeof( CarsRemover ) )]
    [PriorityConductorInformation( PriorityType.None )]
    public class SimpleCarRemoverCondcutor : IConductor
    {
        private CarsRemover _carRemover;
        private bool _canStopOnIt;
        public bool Process( Car car, RoadInformation endDistance )
        {
            throw new System.NotImplementedException();
        }

        public void SetRouteElement( IRoadElement element )
        {
            var carRemover = element as CarsRemover;
            if ( carRemover == null ) { throw new ArgumentException( "Wrong road element" ); }
            this._carRemover = carRemover;
        }

        public void SetCanStopOnIt( bool canStopOnIt )
        {
            this._canStopOnIt = canStopOnIt;
        }

        public IRoadInformation RoadInformation
        {
            get { return this._carRemover.RoadInformation; }
        }

        public IRoadElement RoadElement
        {
            get { return this._carRemover; }
        }
    }
}
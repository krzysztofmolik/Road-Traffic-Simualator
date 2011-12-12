using System;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors.Infrastructure;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors
{
    [ConductorSupportedRoadElementType( typeof( CarsInserter ) )]
    [PriorityConductorInformation( PriorityType.None )]
    public class SimpleCarInserterCondcutor : IConductor
    {
        private bool _canStopOnIt;
        private CarsInserter _carInserter;

        public RoadInformation Process( Car car, IRouteMark<IConductor> route )
        {
            return RoadInformation.Empty;
        }

        public void SetRouteElement( IRoadElement element )
        {
            var carInserter = element as CarsInserter;
            if ( carInserter == null ) { throw new ArgumentException( "Wrong road element" ); }
            this._carInserter = carInserter;
        }

        public void SetCanStopOnIt( bool canStopOnIt )
        {
            this._canStopOnIt = canStopOnIt;
        }

        public IRoadInformation Information
        {
            get { return this._carInserter.RoadInformation; }
        }

        public IRoadElement RoadElement
        {
            get { return this._carInserter; }
        }
    }
}
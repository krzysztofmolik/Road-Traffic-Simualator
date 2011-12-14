using System;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors.Infrastructure;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors
{
    [ConductorSupportedRoadElementType( typeof( LaneCorner ) )]
    [PriorityConductorInformation( PriorityType.None )]
    public class SimpleLanerCornerCondcutor : IConductor
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
                       };
        }

        public void SetRouteElement( IRoadElement element )
        {
            var laneCorner = element as LaneCorner;
            if ( laneCorner == null ) { throw new ArgumentException( "Wrong road element" ); }
            this._laneCorner = laneCorner;
        }

        public void SetCanStopOnIt( bool canStopOnIt )
        {
            this._canStopOnIt = canStopOnIt;
        }

        public IRoadInformation Information
        {
            get {return this._laneCorner.RoadInformation; }
        }

        public IRoadElement RoadElement
        {
            get { return this._laneCorner; }
        }
    }
}
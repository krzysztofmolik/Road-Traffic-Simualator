using System;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Route;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors
{
    public abstract class JunctionConductorBase : IConductor
    {
        private LaneJunction _junction;
        private bool _canStopOnIt;

        public RoadInformation Process( Car car, IRouteMark<IConductor> route )
        {
            var carAheadInformation = this.Information.GetCarAheadDistance( car );
            return new RoadInformation
                       {
                           CarAhead = carAheadInformation.CarAhead,
                           CarAheadDistance = carAheadInformation.CarDistance,
                           PrivilagesCarInformation = this.GetPriorityCarInfromation( car, route ),
                       };
        }

        protected abstract PriorityInformation[] GetPriorityCarInfromation( Car car, IRouteMark<IConductor> route );

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
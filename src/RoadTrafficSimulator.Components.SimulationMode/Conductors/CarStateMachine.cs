using System;
using System.Diagnostics.Contracts;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors
{
    public class CarStateMachine
    {
        private IConductor _currentConductor;
        private readonly Car _car;

        public CarStateMachine( Car car )
        {
            Contract.Requires( car != null );
            this._car = car;
        }

        public void Update( TimeSpan timeFrame )
        {
            if ( this._currentConductor.SholdChange( this._car.Location, this._car.Direction ) )
            {
                this.ChangeConductor( this._car.Route.Dequeue() );
                this.Update( timeFrame );
                return;
            }

            this.StopLineDistance( this._currentConductor.GetDistanceToStopLine() );
            this.LightDistance( this._currentConductor.GetLightInformation() );
            this.YieldDistance( this._currentConductor.GetNextJunctionInformation() );
            this.CarAheadDistance( this._currentConductor.GetCarAheadDistance() );
        }

        private void CarAheadDistance( CarInformation carAhead )
        {
            //            throw new NotImplementedException();
        }

        private void YieldDistance( JunctionInformation nextJunction )
        {
            //            throw new NotImplementedException();
        }

        private void LightDistance( LightInfomration nextLight )
        {
            //            throw new NotImplementedException();
        }

        private void StopLineDistance( float stopLineDistance )
        {
            //            throw new NotImplementedException();
        }

        private void ChangeConductor( IRoadElement roadElement )
        {
            this._car.Direction = roadElement.BuildControl.Location;
            this._currentConductor = roadElement.Condutor;
        }
    }
}
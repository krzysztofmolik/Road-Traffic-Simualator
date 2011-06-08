using System;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using System.Linq;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors
{
    public class CarStateMachine
    {
        private IConductor _currentConductor;
        private IRoadElement _curentRoadElement;
        private readonly Car _car;

        public CarStateMachine( Car car )
        {
            Contract.Requires( car != null );
            this._car = car;
        }

        public void Update( TimeSpan timeFrame )
        {
            if ( this._currentConductor == null )
            {
                this._currentConductor = this._car.Route.First().Condutor;
                this._curentRoadElement = this._car.Route.First();
                this._car.Location = this._curentRoadElement.BuildControl.Location;
            }

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
            this.MoveCar( timeFrame.Milliseconds );
        }

        private void MoveCar( int elesedMs )
        {
            this._car.Location += this._car.Direction * ( this._car.Velocity * elesedMs );
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
            this._currentConductor.Remove( this._car );
            this._currentConductor = roadElement.Condutor;
            this._currentConductor.Take( this._car );
            var direction = this._currentConductor.GetCarDirection( this._car );
            if ( direction == Vector2.Zero )
            {
                this._car.Direction = direction;
            }
            else
            {
                this._car.Direction = Vector2.Normalize( this._currentConductor.GetCarDirection( this._car ) );
            }
        }
    }
}
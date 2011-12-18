using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations
{
    public class DriverBrain
    {
        private readonly Car _car;
        private readonly IEngine _engine;
        private float _destinationSpeed;
        private float _destinationPosition;
        private bool _canDriver;

        public DriverBrain( Car car, IEngine engine )
        {
            this._car = car;
            this._engine = engine;
        }

        public void Process( TimeSpan timeFrame )
        {
            this._destinationPosition = float.MaxValue;
            this._destinationSpeed = this._car.MaxSpeed;
            this._canDriver = true;

            this._canDriver = true;
            var road = this._car.Conductors.Clone();
            var end = this._car.Conductors.Current.Process( this._car, road );

            var distance = 0.0f;
            this.ProcessAnser( end, distance );


            distance += this._car.Conductors.Current.GetCarDistanceToEnd( this._car );

            while ( this._canDriver && road.MoveNext() )
            {
                var processInformation = road.Current.Process( this._car, road );
                this.ProcessAnser( processInformation, distance );

                distance += this.GetLenght( road );
            }

            this._engine.SetStopPoint( this._destinationPosition, this._destinationSpeed );
            this._engine.MoveCar( this._car, timeFrame.Milliseconds );
        }

        private float GetLenght( IRouteMark<IConductor> road )
        {
            return road.Current.RouteElement.Length;
        }

        private void ProcessAnser( RoadInformation end, float distance )
        {
            if ( end.CarAhead != null )
            {
                this.ProcessCarAheadInformation( end.CarAhead, end.CarAheadDistance, distance );
            }

            if ( end.PrivilagesCarInformation != null )
            {
                this.ProcesPrivilagesCarInformation( end.PrivilagesCarInformation, distance );
            }
        }

        private void ProcesPrivilagesCarInformation( PriorityInformation[] privilagesCarInformation, float distance )
        {
            if ( privilagesCarInformation.Length != 0 )
            {
                this.SetDestinationPositionAndSpeed( distance - this._car.Lenght/2 - UnitConverter.FromMeter( 1 ), 0.0f );
                this._canDriver = false;
            }
        }

        private void ProcessCarAheadInformation( Car carAhead, float carAheadDistance, float distance )
        {
            if ( distance + carAheadDistance - carAhead.Lenght < UnitConverter.FromMeter( .9f ) )
            {
//                Debugger.Break();
            }
            this.SetDestinationPositionAndSpeed( distance + carAheadDistance - carAhead.Lenght, carAhead.Velocity );
            this._canDriver = false;
        }

        private void SetDestinationPositionAndSpeed( float distance, float speed )
        {
            var newdestinationPosition = Math.Max( 0, distance - UnitConverter.FromMeter( 1 ) );
            this._destinationPosition = Math.Min( this._destinationPosition, newdestinationPosition );
            this._destinationSpeed = Math.Min( this._destinationSpeed, speed );
        }
    }
}
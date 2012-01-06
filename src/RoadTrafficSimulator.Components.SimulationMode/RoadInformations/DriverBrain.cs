using System;
using System.Diagnostics;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using RoadTrafficSimulator.Infrastructure;
using System.Linq;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations
{
    // TODO Rewrite this
    public class DriverBrain
    {
        private readonly Car _car;
        private readonly IEngine _engine;
        private float _destinationSpeed;
        private float _destinationPosition;
        private bool _canDriver;
        private double _brekingForce;
        private bool _canEntry;
        private float _finalPosition;
        private float _finalSpeed;
        private bool _freeWay;

        public DriverBrain( Car car, IEngine engine )
        {
            this._car = car;
            this._engine = engine;
        }

        public void Process( TimeSpan timeFrame )
        {
            this._destinationPosition = float.MaxValue;
            this._brekingForce = 0.0f;
            this._destinationSpeed = this._car.MaxSpeed;
            this._finalPosition = 0.0f;
            this._canDriver = true;
            this._freeWay = true;

            this._canDriver = true;
            var road = this._car.Conductors.Clone();
            var end = this._car.Conductors.Current.Process( this._car, road );

            var distance = 0.0f;
            this.ProcessAnser( end, distance );

            distance += this._car.Conductors.Current.GetCarDistanceToEnd( this._car );

            this._finalPosition = Math.Min( this._destinationPosition, distance - this._car.Lenght / 2 );
            this._finalSpeed = this._destinationSpeed;


            while ( this._canDriver && road.MoveNext() )
            {
                this._canEntry = true;
                var processInformation = road.Current.Process( this._car, road );

                this.ProcessAnser( processInformation, distance );

                if ( !this._canDriver )
                {
                    if ( !this._canEntry || !processInformation.CanStop )
                    {
                        this._finalSpeed = 0.0f;
                    }
                    else
                    {
                        this._finalPosition = this._destinationPosition;
                        this._finalSpeed = this._destinationSpeed;
                    }
                }
                else if ( processInformation.CanStop )
                {
                    this._finalPosition = distance - this._car.Lenght/2;
                    this._finalSpeed = this._car.MaxSpeed;
                }

                distance += this.GetLenght( road );
            }


            if ( this._freeWay )
            {
                this._engine.SetStopPoint( float.MaxValue, float.MaxValue, this._car );
            }
            else
            {
                this._engine.SetStopPoint( this._finalPosition, this._finalSpeed, this._car );
            }
            this._engine.MoveCar( this._car, timeFrame.Milliseconds );
        }

        private float GetLenght( IRouteMark<IConductor> road )
        {
            return road.Current.RouteElement.Length;
        }

        private void ProcessAnser( RoadInformation end, float distance )
        {
            if ( !end.CanDriver )
            {
                this._freeWay = false;
                this._canDriver = false;
                this.SetDestinationPositionAndSpeed( distance - this._car.Lenght / 2, 0.0f );
            }

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
                var car =
                    privilagesCarInformation.Min( p => this.GetTimeToDriveThrough( p.CarDistanceToJunction, p.CarWihtPriority.Velocity,
                                                  p.CarWihtPriority.MaxSpeed, p.CarWihtPriority.AccelerateForce ) );

                var myTime = this.GetTimeToDriveThrough( distance, this._car.Velocity, this._car.MaxSpeed, this._car.AccelerateForce );

                if ( myTime + UnitConverter.FromSecond( 2 ) > car )
                {
                    this.SetDestinationPositionAndSpeed( distance - this._car.Lenght / 2 - UnitConverter.FromMeter( 1 ), 0.0f );
                    this._canDriver = false;
                    this._freeWay = false;
                }
            }
        }

        // TODO Change name
        private float GetTimeToDriveThrough( float s, float v, float vMax, float a )
        {
            float t1, s1, t, x1, x2;
            t1 = ( vMax - v ) / a;
            s1 = v * t1 + ( a * t1 * t1 ) / 2;
            if ( s1 > s )
            {
                var del = Math.Max( 0, 2 * a * s - v );
                x1 = ( float ) ( -( v + Math.Sqrt( del ) ) / a );
                x2 = ( float ) ( -( v - Math.Sqrt( del ) / a ) );
                if ( x1 > x2 )
                    return x1;
                else
                    return x2;

            }
            else
            {
                t = t1 + ( s - s1 ) / vMax;
                return t;
            }
        }

        private void ProcessCarAheadInformation( Car carAhead, float carAheadDistance, float distance )
        {
            this._canDriver = false;
            this._freeWay = false;
            this.SetDestinationPositionAndSpeed( distance + carAheadDistance - carAhead.Lenght, carAhead.Velocity );

            if ( carAheadDistance < carAhead.Lenght + UnitConverter.FromMeter( 0.5f ) && carAhead.Velocity < UnitConverter.FromKmPerHour( 5.0f ) )
            {
                this._canEntry = false;
            }
        }

        private void SetDestinationPositionAndSpeed( float distance, float speed )
        {
            var speedDelta = Math.Max( 0, this._car.Velocity - speed );
            var breakingForce = speedDelta / ( distance + 1 );
            if ( this._brekingForce > breakingForce ) { return; }

            this._destinationPosition = Math.Max( 0, distance - UnitConverter.FromMeter( 1 ) );
            this._destinationSpeed = speed;
        }
    }
}
using System;
using System.Linq;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations
{
    public class Engine : IEngine
    {
        private float _stopPointDistance = float.MaxValue;
        private float _requiredSpeed = float.MaxValue;
        private float _brekingForce;

        public void SetStopPoint( float distance, float requriredSpeed, Car car )
        {
            requriredSpeed = Math.Min( requriredSpeed, car.MaxSpeed );
            var speedDelta = Math.Max( 0, car.Velocity - requriredSpeed );
            var breakingForce = speedDelta / ( distance + 10 );
            if ( this._brekingForce > breakingForce ) { return; }
            this._brekingForce = breakingForce;
            this._stopPointDistance = distance;
            this._requiredSpeed = requriredSpeed;
        }

        public void MoveCar( Car car, int elapsedMs )
        {
            var distance = this.GetVelocity( car, elapsedMs );
            var nextPoint = car.Direction * ( distance );
            car.Location += nextPoint;

            this.Clear();
        }

        private void Clear()
        {
            this._stopPointDistance = float.MaxValue;
            this._requiredSpeed = float.MaxValue;
            this._brekingForce = float.MinValue;
        }

        private float GetVelocity( Car car, int elapsedMs )
        {
            if ( this._stopPointDistance <= 0 )
            {
                car.Velocity = 0.0f;
                return 0.0f;
            }

            if ( this._requiredSpeed > car.Velocity )
            {
                return this.Accelerate( car, elapsedMs );
            }

            if ( this._stopPointDistance < UnitConverter.FromMeter( 0.1f ) )
            {
                car.Velocity = this._requiredSpeed;
                return this._stopPointDistance;
            }

            return this.Break( car, elapsedMs );
        }

        private float Accelerate( Car car, int elapsedMs )
        {
            var accelerated = car.AccelerateForce * elapsedMs;
            car.Velocity = Math.Min( car.MaxSpeed, car.Velocity + accelerated );
            return Math.Min( car.Velocity * elapsedMs, this._stopPointDistance );
        }

        private float Break( Car car, int elapsedMs )
        {
            var breakingDistance = GetBreakingDistance( car );
            if ( breakingDistance < this._stopPointDistance - UnitConverter.FromMeter( 1.0f ) )
            {
                this.Accelerate( car, elapsedMs );
            }

            var breakingForce = Math.Pow( car.Velocity - this._requiredSpeed, 2 ) / ( 2 * this._stopPointDistance );

            car.Velocity -= ( float ) breakingForce * elapsedMs;
            return Math.Min( car.Velocity * elapsedMs, this._stopPointDistance );
        }

        private double GetBreakingDistance( Car car )
        {
            var speedDifferenc = car.Velocity - this._requiredSpeed;
            var breakingDistance = Math.Pow( speedDifferenc, 2 ) / ( 2 * car.BreakingForce );
            return breakingDistance;
        }
    }
}
using System;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Light;
using RoadTrafficSimulator.Infrastructure;
using System.Linq;
using RoadTrafficSimulator.Infrastructure.MathHelpers;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors
{
    public class CarStateMachine
    {
        private IConductor _currentConductor;
        private IRoadElement _curentRoadElement;
        private readonly Car _car;
        private ICarMachineState _currentState;

        public CarStateMachine( Car car )
        {
            Contract.Requires( car != null );
            this._currentState = new StopPointCarMachineState();
            this._car = car;
        }

        public void Update( TimeSpan timeFrame )
        {
            if ( this._currentConductor == null )
            {
                this._currentConductor = this._car.Route.Current.Condutor;
                this._curentRoadElement = this._car.Route.Current;
                this._car.Location = this._curentRoadElement.BuildControl.Location;
            }

            if ( this._currentConductor.ShouldChange( this._car.Location, this._car ) )
            {
                this._car.Route.MoveNext();
                this.ChangeConductor( this._car.Route.Current );
                this.Update( timeFrame );
                return;
            }

            this._currentState.SetStopPoint( float.MaxValue, this._car.MaxSpeed );
            this.StopLineDistance( this._currentConductor.GetDistanceToStopLine() );
            this.LightDistance( this.GetLightInformation() );
            this.YieldDistance( this.GetYieldInfomration() );
            this.CarAheadDistance( this.GetCarAheadInformation() );
            this._currentState.MoveCar( this._car, timeFrame.Milliseconds );
        }

        private JunctionInformation GetYieldInfomration()
        {
            var junctionInformation = new JunctionInformation();
            junctionInformation.JunctionDistance = this._currentConductor.GetCarDistanceToEnd( this._car );
            // TODO Rewrite it
            if ( this._car.Route.IsLast ) { return junctionInformation; }
            var route = this._car.Route.Clone();
            route.MoveNext();
            route.Current.Condutor.GetNextJunctionInformation( route, junctionInformation );

            return junctionInformation;
        }

        private CarInformation GetCarAheadInformation()
        {
            var carInformation = new CarInformation();
            carInformation.QuestioningCar = this._car;
            this._currentConductor.GetCarAheadDistance( this._car.Route.Clone(), carInformation );
            return carInformation;
        }

        private LightInfomration GetLightInformation()
        {
            var lightInformation = new LightInfomration();
            var routeMark = this._car.Route.Clone();
            var next = routeMark.GetNext();
            if ( next == null ) { return lightInformation; }
            lightInformation.LightDistance = this._currentConductor.GetCarDistanceToEnd( this._car );
            lightInformation.Car = this._car;
            this._currentConductor.GetLightInformation( routeMark, lightInformation );
            return lightInformation;
        }

        private void CarAheadDistance( CarInformation carAheadInformation )
        {
            if ( carAheadInformation.CarAhead != null )
            {
                var carAhead = carAheadInformation.CarAhead;
                var distance = carAheadInformation.CarDistance - ( carAhead.Lenght / 2 ) - ( this._car.Lenght / 2 ) - UnitConverter.FromMeter( Math.Min( this._car.Velocity, carAhead.Velocity ) / UnitConverter.FromKmPerHour( 10.0f ) );
                this._currentState.SetStopPoint( distance, carAhead.Velocity );
            }
            else
            {
                this._currentState.SetStopPoint( float.MaxValue, float.MaxValue );
            }
        }

        private void YieldDistance( JunctionInformation nextJunction )
        {
            if ( nextJunction.AdditionalCars.Any() == false ) { return; }

            var car = nextJunction.AdditionalCars.Aggregate( ( prev, current ) => prev.JunctionDistance > current.JunctionDistance ? current : prev );
            if ( car == null ) { return; }

            var time = MyMathHelper.GetTimeNeedToDriverDistanceInUniformlyAcceleratedMontion( car.Car.Velocity, car.Car.MaxSpeed, car.Car.AccelerateForce, car.CarDistance );
            var distance = MyMathHelper.DistanceInUniformlyAcceleratedMotion( this._car.Velocity, this._car.MaxSpeed, time, this._car.AccelerateForce );
            if ( distance < car.JunctionDistance + UnitConverter.FromMeter( 20 ) )
            {
                this._currentState.SetStopPoint( car.JunctionDistance, 0.0f );
            }
        }

        private void LightDistance( LightInfomration nextLight )
        {
            if ( nextLight.LightState != LightState.Green )
            {
                this._currentState.SetStopPoint( nextLight.LightDistance - UnitConverter.FromMeter( 1.0f ), 0.0f );
            }
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

    public interface ICarMachineState
    {
        void SetStopPoint( float distance, float requriredSpeed );
        void MoveCar( Car car, int elapsedMs );
    }

    public class StopPointCarMachineState : ICarMachineState
    {
        private float _stopPointDistance;
        private float _requiredSpeed;

        public void SetStopPoint( float distance, float requriredSpeed )
        {
            if ( this._stopPointDistance < distance ) { return; }
            this._stopPointDistance = distance;
            this._requiredSpeed = requriredSpeed;
        }

        public void MoveCar( Car car, int elapsedMs )
        {
            var distance = this.GetVelocity( car, this._stopPointDistance, this._requiredSpeed, elapsedMs );
            var nextPoint = car.Direction * ( distance );
            car.Location += nextPoint;

            this.Clear();
        }

        private void Clear()
        {
            this._stopPointDistance = float.MaxValue;
            this._requiredSpeed = float.MaxValue;
        }

        private float GetVelocity( Car car, float distance, float requiredSpeed, int elapsedMs )
        {
            if ( distance < 0 )
            {
                car.Velocity = 0.0f;
                return 0.0f;
            }

            if ( requiredSpeed > car.Velocity )
            {
                return this.Accelerate( car, elapsedMs );
            }

            if ( distance < UnitConverter.FromMeter( 0.1f ) )
            {
                car.Velocity = requiredSpeed;
                return distance;
            }

            return this.Break( car, elapsedMs );
        }

        private float Accelerate( Car car, int elapsedMs )
        {
            var accelerated = car.AccelerateForce * elapsedMs;
            car.Velocity = car.Velocity + accelerated;
            return Math.Min( car.Velocity * elapsedMs, this._stopPointDistance );
        }

        private float DontChangeSpeed( Car car, int elapsedMs )
        {
            return car.Velocity * elapsedMs;
        }

        private float Break( Car car, int elapsedMs )
        {
            var speedDifferenc = car.Velocity - this._requiredSpeed;
            var breakingDistance = Math.Pow( speedDifferenc, 2 ) / ( 2 * car.BreakingForce );
            if ( breakingDistance < this._stopPointDistance - UnitConverter.FromMeter( 1.0f ) )
            {
                var accelerated = car.AccelerateForce * elapsedMs;
                car.Velocity = car.Velocity + accelerated;
                return Math.Min( car.Velocity * elapsedMs, this._stopPointDistance );
            }

            car.Velocity -= car.BreakingForce * elapsedMs;
            return Math.Min( car.Velocity * elapsedMs, this._stopPointDistance );
        }
    }
}
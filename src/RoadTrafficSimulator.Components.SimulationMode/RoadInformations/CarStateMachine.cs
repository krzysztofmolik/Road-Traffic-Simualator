using System;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Light;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.MathHelpers;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors
{
    public class CarStateMachine
    {
        private IRoadInformation _currentRoadInformation;
        private IRoadElement _curentRoadElement;
        private readonly Car _car;
        private IEngine _currentState;

        public CarStateMachine( Car car )
        {
            Contract.Requires( car != null );
            this._currentState = new Engine();
            this._car = car;
        }

        public void Update( TimeSpan timeFrame )
        {
            if ( this._currentRoadInformation == null )
            {
                this._currentRoadInformation = this._car.RoadElements.Current.RoadInformation;
                this._curentRoadElement = this._car.RoadElements.Current;
                this._car.Location = this._curentRoadElement.BuildControl.Location;
            }

            if ( this._currentRoadInformation.ShouldChange(this._car ) )
            {
                this._car.RoadElements.MoveNext();
                this.ChangeConductor( this._car.RoadElements.Current );
                this.Update( timeFrame );
                return;
            }

            this._currentState.SetStopPoint( float.MaxValue, this._car.MaxSpeed );
            this.StopLineDistance( this._currentRoadInformation.GetDistanceToStopLine() );
            this.LightDistance( this.GetLightInformation() );
            this.YieldDistance( this.GetYieldInfomration() );
            this.CarAheadDistance( this.GetCarAheadInformation() );
            this.AvailablePointsToStop( this.GetAvailablePointsToStop() );
            this._currentState.MoveCar( this._car, timeFrame.Milliseconds );
        }

        private NextAvailablePointToStopInfo GetAvailablePointsToStop()
        {
            var iterator = new NextAvailavlePointToStopInfoIterator();
            return iterator.Get( this._car.RoadElements.Clone(), 100.0f, this._car );
        }

        private void AvailablePointsToStop( NextAvailablePointToStopInfo availablePointToStop )
        {
            this._currentState.SetAvailablePointsToStop( availablePointToStop );
        }

        private JunctionInformation GetYieldInfomration()
        {
            var junctionInformation = new JunctionInformation();
            junctionInformation.JunctionDistance = this._currentRoadInformation.GetCarDistanceToEnd( this._car );
            // TODO Rewrite it
            if ( this._car.RoadElements.IsLast ) { return junctionInformation; }
            var route = this._car.RoadElements.Clone();
            route.MoveNext();
            route.Current.RoadInformation.GetNextJunctionInformation( route, junctionInformation );

            return junctionInformation;
        }

        private CarInformation GetCarAheadInformation()
        {
            var carInformation = new CarInformation();
            carInformation.QuestioningCar = this._car;
            this._currentRoadInformation.GetCarAheadDistance( this._car.RoadElements.Clone(), carInformation );
            return carInformation;
        }

        private LightInfomration GetLightInformation()
        {
            var lightInformation = new LightInfomration();
            var routeMark = this._car.RoadElements.Clone();
            var next = routeMark.GetNext();
            if ( next == null ) { return lightInformation; }
            lightInformation.LightDistance = this._currentRoadInformation.GetCarDistanceToEnd( this._car );
            lightInformation.Car = this._car;
            this._currentRoadInformation.GetLightInformation( routeMark, lightInformation );
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
            this._currentRoadInformation.OnExit( this._car );
            this._currentRoadInformation = roadElement.RoadInformation;
            this._currentRoadInformation.OnEnter( this._car );
            var direction = this._currentRoadInformation.GetCarDirection( this._car );
            if ( direction == Vector2.Zero )
            {
                this._car.Direction = direction;
            }
            else
            {
                this._car.Direction = Vector2.Normalize( this._currentRoadInformation.GetCarDirection( this._car ) );
            }
        }
    }
}
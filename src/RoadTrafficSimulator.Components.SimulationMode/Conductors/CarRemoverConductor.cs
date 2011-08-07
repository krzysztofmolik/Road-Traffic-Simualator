using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Common;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Messages;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors
{
    public class CarRemoverConductor : IConductor
    {
        private readonly CarsRemover _carsRemover;
        private readonly Queue<Car> _cars = new Queue<Car>();
        private readonly IEventAggregator _eventAggregator;

        public CarRemoverConductor( CarsRemover carsRemover, IEventAggregator eventAggregator )
        {
            Contract.Requires( carsRemover != null );
            Contract.Requires( eventAggregator != null );
            this._carsRemover = carsRemover;
            this._eventAggregator = eventAggregator;
        }

        public IRoadElement GetNextRandomElement( List<IRoadElement> route, Random rng )
        {
            return null;
        }

        public void Take( Car car )
        {
            this._eventAggregator.Publish( new CarRemoved( car ) );
        }

        public void Remove(Car car)
        {
            var removedCar = this._cars.Dequeue();
            Debug.Assert( car == removedCar );
        }

        public float GetCarDistanceToEnd(Car car)
        {
            if( this._cars.Contains(car))
            {
                return Constans.PointSize;
            }

            return float.MaxValue;
        }

        public bool IsPosibleToDriveFrom( IRoadElement roadElement )
        {
            return this._carsRemover.Lane == roadElement;
        }

        public bool IsPosibleToDriveTo( IRoadElement roadElement )
        {
            return false;
        }

        public bool ShouldChange(Vector2 acutalCarLocation, Car car)
        {
            return false;
        }

        public float GetDistanceToStopLine()
        {
            return float.MaxValue;
        }

        public void GetLightInformation(IRouteMark routeMark, LightInfomration lightInformation)
        {
            lightInformation.LightDistance = float.MaxValue;
        }

        public void GetNextJunctionInformation( IRouteMark route, JunctionInformation junctionInformation )
        {
            junctionInformation.JunctionDistance = float.MaxValue;
        }

        public void GetCarAheadDistance(IRouteMark routMark, CarInformation carInformation)
        {
            carInformation.CarDistance = float.MaxValue;
            carInformation.CarAhead = null;
        }

        public void GetFirstCarToOutInformation( FirstCarToOutInformation carInformation )
        {
            Debug.Assert( false );
        }

        public Vector2 GetCarDirection(Car car)
        {
            return new Vector2( 0, 0 );
        }

    }
}
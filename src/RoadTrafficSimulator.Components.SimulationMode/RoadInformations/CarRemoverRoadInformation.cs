using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Common;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Messages;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations
{
    public class CarRemoverRoadInformation : IRoadInformation
    {
        private readonly CarsRemover _carsRemover;
        private readonly Queue<Car> _cars = new Queue<Car>();
        private readonly IEventAggregator _eventAggregator;

        public CarRemoverRoadInformation( CarsRemover carsRemover, IEventAggregator eventAggregator )
        {
            Contract.Requires( carsRemover != null );
            Contract.Requires( eventAggregator != null );
            this._carsRemover = carsRemover;
            this._eventAggregator = eventAggregator;
        }

        public void OnEnter( Car car )
        {
            this._eventAggregator.Publish( new CarRemoved( car ) );
        }

        public void OnExit(Car car)
        {
            var removedCar = this._cars.Dequeue();
            Debug.Assert( car == removedCar );
        }

        public float Lenght(IRoadElement previous, IRoadElement next)
        {
            return Constans.PointSize;
        }

        public bool CanStop(IRoadElement previous, IRoadElement next)
        {
            return true;
        }

        public bool ShouldChange(Car car)
        {
            return false;
        }

        public void GetCarAheadDistance(IRouteMark<IRoadElement> routMark, CarInformation carInformation)
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
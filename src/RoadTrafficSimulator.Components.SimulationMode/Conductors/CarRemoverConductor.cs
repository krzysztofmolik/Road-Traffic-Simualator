using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Common;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Messages;

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

        public IRoadElement GetNextRandomElement()
        {
            return null;
        }

        public void Take( Car car )
        {
            this._eventAggregator.Publish( new CarRemoved( car ) );
        }

        public void RemoveCar( Car car )
        {
            throw new InvalidOperationException();
        }

        public bool SholdChange( Vector2 acutalCarLocation, Vector2 direction )
        {
            return false;
        }

        public float GetDistanceToStopLine()
        {
            return float.MaxValue;
        }

        public LightInfomration GetLightInformation()
        {
            return new LightInfomration { LightDistance = float.MaxValue };
        }

        public JunctionInformation GetNextJunctionInformation()
        {
            return new JunctionInformation { JunctionDistance = float.MaxValue };
        }

        public CarInformation GetCarAheadDistance()
        {
            return new CarInformation() { CarDistance = float.MaxValue };
        }
    }
}
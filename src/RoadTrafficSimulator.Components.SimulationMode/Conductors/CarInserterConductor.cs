using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors
{
    public class CarInserterConductor : IConductor
    {
        private readonly CarsInserter _carInserter;
        private readonly Queue<Car> _cars = new Queue<Car>();

        public CarInserterConductor( CarsInserter carInserter )
        {
            Contract.Requires( carInserter != null );
            this._carInserter = carInserter;
        }

        public IRoadElement GetNextRandomElement()
        {
            return this._carInserter.Lane;
        }

        public void Take( Car car )
        {
            this._cars.Enqueue( car );
        }

        public void Remove( Car car )
        {
            var removedCar = this._cars.Dequeue();
            Debug.Assert( car == removedCar );
        }

        public bool SholdChange( Vector2 acutalCarLocation, Vector2 direction )
        {
            return true;
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
            return new CarInformation { CarDistance = float.MaxValue };
        }

        public Vector2 GetCarDirection( Car car )
        {
            return this._carInserter.Lane.Condutor.GetCarDirection( car );
        }
    }
}
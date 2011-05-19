using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors
{
    public class CarInserterConductor : IConductor
    {
        private readonly IRoadElement _carInserter;
        private Queue<Car> _cars = new Queue<Car>();

        public CarInserterConductor( IRoadElement carInserter )
        {
            Contract.Requires( carInserter != null );
            this._carInserter = carInserter;
        }

        public IRoadElement GetNextRandomElement()
        {
            return this._carInserter;
        }

        public void Take( Car car )
        {
            this._cars.Enqueue( car );
        }

        public bool SholdChange(Vector2 acutalCarLocation, Vector2 direction)
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
    }
}
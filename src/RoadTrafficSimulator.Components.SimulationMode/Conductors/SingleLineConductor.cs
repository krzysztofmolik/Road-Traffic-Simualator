using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors
{
    public class SingleLineConductor : IConductor
    {
        private readonly Lane _lane;
        private readonly Queue<Car> _cars = new Queue<Car>();

        public SingleLineConductor( Lane lane )
        {
            Contract.Requires( lane != null );
            this._lane = lane;
        }

        public IRoadElement GetNextRandomElement()
        {
            Debug.Assert( this._lane.Top == null );
            Debug.Assert( this._lane.Bottom == null );
            return this._lane.Next;
        }

        public void Take( Car car )
        {
            Contract.Requires( car != null );
            this._cars.Enqueue( car );
        }

        public void RemoveCar( Car car )
        {
            var removedCar = this._cars.Dequeue();
            Debug.Assert( removedCar == car );
        }

        public bool SholdChange( Vector2 acutalCarLocation, Vector2 direction )
        {
            var distance = this._lane.Next.BuildControl.Location - acutalCarLocation;
            // TODO Check value and extract some kind of property
            if ( distance.Length() <= 0.001f ) { return true; }

            return Math.Sign( distance.X ) == Math.Sign( direction.X ) && Math.Sign( distance.Y ) == Math.Sign( direction.Y );
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
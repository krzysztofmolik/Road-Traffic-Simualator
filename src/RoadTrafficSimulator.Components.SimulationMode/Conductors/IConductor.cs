using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Light;
using RoadTrafficSimulator.Components.SimulationMode.Route;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors
{
    public interface IConductor
    {
        IRoadElement GetNextRandomElement( List<IRoadElement> route, Random rng );
        void Take( Car car );
        bool ShouldChange( Vector2 acutalCarLocation, Car car );
        float GetDistanceToStopLine();
        void GetLightInformation( IRouteMark routeMark, LightInfomration lightInformation );
        void GetNextJunctionInformation( IRouteMark route, JunctionInformation junctionInformation );
        void GetCarAheadDistance( IRouteMark routMark, CarInformation carInformation );
        void GetFirstCarToOutInformation( FirstCarToOutInformation carInformation );
        Vector2 GetCarDirection( Car car );
        void Remove( Car car );
        float GetCarDistanceToEnd( Car car );
        bool IsPosibleToDriveFrom( IRoadElement roadElement );
        bool IsPosibleToDriveTo( IRoadElement roadElement );
    }

    public class LightInfomration
    {
        public Car Car { get; set; }
        public float LightDistance { get; set; }
        public LightState LightState { get; set; }
    }

    public class JunctionInformation
    {
        public class Item
        {
            public Item( Car car, float carDistance, float junctionDistance, bool isAbigouos )
            {
                this.Car = car;
                this.CarDistance = carDistance;
                this.JunctionDistance = junctionDistance;
                this.IsAmbiguous = isAbigouos;
            }

            public Car Car { get; private set; }
            public float CarDistance { get; private set; }
            public bool IsAmbiguous { get; private set; }
            public float JunctionDistance { get; private set; }
        }

        private readonly List<Item> _aditionalCars = new List<Item>();

        public float JunctionDistance { get; set; }
        public IEnumerable<Item> AdditionalCars { get { return this._aditionalCars; } }

        public void AddCar( Car car, float carDistance, float junctionDistance, bool isAbigouous = false )
        {
            this._aditionalCars.Add( new Item( car, carDistance, junctionDistance, isAbigouous ) );
        }
    }
}
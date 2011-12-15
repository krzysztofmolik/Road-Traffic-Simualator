using System.Collections.Generic;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations
{
    public class FirstCarToOutInformation
    {
        private static readonly FirstCarToOutInformation _empty = new FirstCarToOutInformation( null, float.MaxValue );
        public static FirstCarToOutInformation Empty
        {
            get { return _empty; }
        }

        public FirstCarToOutInformation( Car car, float carDistance )
        {
            this.Car = car;
            this.CarDistance = carDistance;
        }

        public Car Car { get; private set; }
        public float CarDistance { get; private set; }

        public float Distance { get; set; }
    }
}
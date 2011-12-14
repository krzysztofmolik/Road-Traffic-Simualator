using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations
{
    public class CarAhedInformation
    {
        private static CarAhedInformation _empty = new CarAhedInformation { CarAhead = null, CarDistance = float.MaxValue };
        public static CarAhedInformation Empty { get { return _empty; } }

        public float CarDistance { get; set; }
        public Car CarAhead { get; set; }
    }
}
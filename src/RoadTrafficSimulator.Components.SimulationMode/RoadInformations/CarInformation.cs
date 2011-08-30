using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors
{
    public class CarInformation
    {
        public float CarDistance { get; set; }
        public Car QuestioningCar { get; set; }
        public Car CarAhead { get; set; }
    }
}
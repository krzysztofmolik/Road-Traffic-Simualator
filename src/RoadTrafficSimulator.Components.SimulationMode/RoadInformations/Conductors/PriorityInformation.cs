using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors
{
    public class PriorityInformation
    {
        public PriorityInformation( Car carWihtPriority, float carDistanceToJunction, float distanceToJunction )
        {
            this.CarWihtPriority = carWihtPriority;
            this.CarDistanceToJunction = carDistanceToJunction;
            this.DistanceToJunction = distanceToJunction;
        }

        public Car CarWihtPriority { get; private set; }
        public float CarDistanceToJunction { get; private set; }
        public float DistanceToJunction { get; private set; }
    }
}
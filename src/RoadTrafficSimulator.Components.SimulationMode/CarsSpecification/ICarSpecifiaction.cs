using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.CarsSpecification
{
    public interface ICarSpecifiaction
    {
        Car Create( IRoadElement startElement );
    }
}
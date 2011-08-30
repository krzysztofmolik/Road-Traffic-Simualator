using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations
{
    public interface IEngine
    {
        void SetStopPoint( float distance, float requriredSpeed );
        void MoveCar( Car car, int elapsedMs );
    }
}
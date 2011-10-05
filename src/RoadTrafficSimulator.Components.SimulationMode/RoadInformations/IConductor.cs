using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations
{
    public interface IConductor
    {
        bool ShouldChange( Car car );
        float GetDistanceToEnd(Car car);
        bool Process(Car car, RoadInformation endDistance);
        float Length { get; }
        void OnExit(Car car);
        void OnEnter(Car car);
        Vector2 GetCarDirection(Car car);
    }
}
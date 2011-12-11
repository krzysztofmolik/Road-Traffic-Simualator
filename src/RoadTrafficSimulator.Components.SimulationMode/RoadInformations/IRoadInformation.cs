using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Route;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations
{
    public interface IRoadInformation
    {
        void OnEnter( Car car );
        void OnExit(Car car);
        float Lenght(IRoadElement previous, IRoadElement next);
        bool ShouldChange( Car car );
        void GetCarAheadDistance(IRouteMark<IRoadElement> routMark, CarInformation carInformation);
        void GetFirstCarToOutInformation( FirstCarToOutInformation carInformation );
        Vector2 GetCarDirection( Car car, IRoadElement nextPoint );
        float GetCarDistanceTo( Car car, IRoadElement nextPoint );
    }
}

using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Route;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations
{
    public interface IRoadInformation
    {
        void OnEnter( Car car );
        void OnExit(Car car);
        bool ShouldChange( Car car );
        CarAhedInformation GetCarAheadDistance( Car car );
        FirstCarToOutInformation GetFirstCarToOutInformation();
        bool ContainsCar( Car car );
    }
}

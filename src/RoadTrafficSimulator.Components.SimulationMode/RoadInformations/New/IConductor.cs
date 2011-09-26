using RoadTrafficSimulator.Components.SimulationMode.Controlers;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.New
{
    public interface IConductor
    {
        void OnExit( Car car );
        void OnEnter( Car car );
        bool ShouldChange( Car car );
        float GetStopPoint(Car car);
    }
}

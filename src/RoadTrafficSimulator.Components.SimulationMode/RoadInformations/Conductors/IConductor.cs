using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors
{
    public interface IConductor
    {
        bool Process( Car car, RoadInformation endDistance );
        void SetRouteElement( IRoadElement element );
        void SetCanStopOnIt( bool canStopOnIt );
        IRoadInformation RoadInformation { get; }
        IRoadElement RoadElement { get; }
    }
}
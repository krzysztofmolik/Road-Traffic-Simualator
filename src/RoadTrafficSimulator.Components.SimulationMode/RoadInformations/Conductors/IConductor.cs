using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Route;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors
{
    public interface IConductor
    {
        RoadInformation Process( Car car, IRouteMark<IConductor> route );
        void SetRouteElement( IRoadElement element );
        void SetCanStopOnIt( bool canStopOnIt );
        IRoadInformation Information { get; }
        IRoadElement RoadElement { get; }
    }
}
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Builder;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors
{
    public interface IConductor
    {
        RoadInformation Process( Car car, IRouteMark<IConductor> route );
        IRoadInformation Information { get; }
        RouteElement RouteElement { get; }
        void Setup( RouteElement roadElement, bool canStopOnIt, IRoadElement previous, IRoadElement next, PriorityType priorityType );
        Vector2 GetCarDirection( Car car );
        float GetCarDistanceToEnd( Car car );
    }
}
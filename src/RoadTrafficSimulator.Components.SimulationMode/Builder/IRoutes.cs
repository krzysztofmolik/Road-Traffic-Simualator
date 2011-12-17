using System;
using System.Collections.Generic;
using RoadTrafficSimulator.Components.SimulationMode.Route;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public interface IRoutes
    {
        IEnumerable<RouteElement> GetRandomRoute( Random rng );
        void CalculateProbabilities();
        void Add( BuildRoute route );
        void AddRoadThatBelongToIt( BuildRoute convertedRoutes, IRouteMark<RouteElement> routeMark );
        IEnumerable<BelongToRouteItem> BelongToRoutes { get; }
        IEnumerable<BuildRoute> AvailableRoutes { get; }
    }
}
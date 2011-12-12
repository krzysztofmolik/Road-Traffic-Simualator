using System;
using System.Collections.Generic;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public interface IRoutes
    {
        IEnumerable<RouteElement> GetRandomRoute( Random rng, RouteElement oneBeforeLast );
        void CalculateProbabilities();
        void Add( BuildRoute route );
    }
}
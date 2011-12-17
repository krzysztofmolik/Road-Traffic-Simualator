using System.Collections.Generic;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public class BuildRoute
    {
        private readonly List<RouteElement> _elements;

        public BuildRoute( IEnumerable<RouteElement> elements )
        {
            this._elements = new List<RouteElement>( elements );
        }

        public IEnumerable<RouteElement> Elements { get { return this._elements; } }
        public float Probability { get; set; }
        public string Name { get; set; }
        public IRoadElement Owner { get; set; }
    }
}
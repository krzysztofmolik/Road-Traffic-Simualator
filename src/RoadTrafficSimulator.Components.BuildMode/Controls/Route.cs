using System.Collections.Generic;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class RouteElement
    {
        public RouteElement(IControl control, PriorityType priorityType)
        {
            this.Control = control;
            this.PriorityType = priorityType;
        }

        public IControl Control { get; set; }
        public PriorityType PriorityType { get; set; }
    }

    public class Route
    {
        private readonly List<RouteElement> _route;

        public Route()
        {
            this._route = new List<RouteElement>();
        }

        public Route( IEnumerable<RouteElement> routeElements, float probability)
        {
            this._route = new List<RouteElement>(routeElements);
            this.Probability = probability;
        }

        public float Probability { get; set; }
        public IEnumerable<RouteElement> Items { get { return this._route; } }
    }
}
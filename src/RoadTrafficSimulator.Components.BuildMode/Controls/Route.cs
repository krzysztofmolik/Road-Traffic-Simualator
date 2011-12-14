using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class RouteElement
    {
        public RouteElement( IControl control, PriorityType priorityType )
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
            : this( Enumerable.Empty<RouteElement>(), 0 )
        {
            this._route = new List<RouteElement>();
        }

        public Route( string name, int probability )
        {
            this.Name = name;
            this.Probability = probability;
            this._route = new List<RouteElement>();
        }

        public Route( IEnumerable<RouteElement> routeElements, int probability )
        {
            this._route = new List<RouteElement>( routeElements );
            this.Probability = probability;
            this.Name = "Unknown";
        }

        public string Name { get; set; }
        public int Probability { get; set; }
        public IEnumerable<RouteElement> Items { get { return this._route; } }

        public bool CanAdd( IControl control )
        {
            if ( this._route.IsEmpty() ) { return true; }

            // NOTE This only fake
            return true;
        }

        public void Add( IControl control, PriorityType priorityType )
        {
            this._route.Add( new RouteElement( control, priorityType ) );
        }

        public IEnumerable<PriorityType> GetPrioritiesFor( IControl control )
        {
            return new[]
                       {
                           PriorityType.Light,
                           PriorityType.FromLeft,
                           PriorityType.FromFront,
                           PriorityType.FromRight,
                       };
        }

        public void Add( RouteElement routeElement )
        {
            this._route.Add( routeElement );
        }
    }
}
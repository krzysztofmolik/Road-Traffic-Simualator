using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using RoadTrafficSimulator.Components.SimulationMode.Route;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public class StandardRoutes : IRoutes
    {
        private readonly static StandardRoutes _empty = new StandardRoutes( Enumerable.Empty<BuildRoute>() );

        public static StandardRoutes Empty
        {
            get { return _empty; }
        }

        private readonly List<BuildRoute> _routes;
        private readonly List<BelongToRouteItem> _routesThatBelong;

        public StandardRoutes( IEnumerable<BuildRoute> routes )
        {
            this._routes = new List<BuildRoute>( routes );
            this._routesThatBelong = new List<BelongToRouteItem>();
        }

        public IEnumerable<RouteElement> GetRandomRoute( Random rng )
        {
            if ( this._routes.IsEmpty() ) { return Enumerable.Empty<RouteElement>(); }
            var maxValue = this._routes.Max( s => s.Probability );
            var rngNumber = rng.Next( 1, ( int ) maxValue );
            var previous = 0.0;
            for ( var i = 0; i < this._routes.Count; i++ )
            {
                if ( previous < rngNumber && rngNumber <= this._routes[ i ].Probability )
                {
                    return this._routes[ i ].Elements;
                }
                previous = this._routes[ i ].Probability;
            }

            throw new InvalidOperationException();
        }

        public void CalculateProbabilities()
        {
            var alreadySet = this._routes.Where( s => s.Probability != 0 ).ToArray();
            var inUse = alreadySet.Sum( s => s.Probability );

            var left = this._routes.Count - alreadySet.Length;
            if ( left == 0 ) { return; }

            var perItem = ( 100 - inUse ) / left;
            this._routes.Where( s => s.Probability == 0 ).ForEach( s => s.Probability = perItem );
        }

        public IEnumerable<BuildRoute> AvailableRoutes { get { return this._routes; } }

        public void Add( BuildRoute route )
        {
            this._routes.Add( route );
        }

        public void AddRoadThatBelongToIt( BuildRoute convertedRoutes, IRouteMark<RouteElement> routeMark )
        {
            this._routesThatBelong.Add( new BelongToRouteItem( convertedRoutes, routeMark ) );
        }

        public IEnumerable<BelongToRouteItem> BelongToRoutes
        {
            get { return this._routesThatBelong; }
        }
    }

    public class BelongToRouteItem
    {
        public BelongToRouteItem( BuildRoute route, IRouteMark<RouteElement> position )
        {
            this.Route = route;
            this.Position = position;
        }

        public BuildRoute Route { get; private set; }
        public IRouteMark<RouteElement> Position { get; private set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Common;

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

        public StandardRoutes( IEnumerable<BuildRoute> routes )
        {
            this._routes = new List<BuildRoute>( routes );
        }

        public IEnumerable<RouteElement> GetRandomRoute( Random rng, RouteElement @from )
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

        public void Add( BuildRoute route )
        {
            this._routes.Add( route );
        }
    }
}
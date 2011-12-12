using System;
using System.Collections.Generic;
using System.Linq;
using RoadTrafficSimulator.Components.SimulationMode.Elements;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public class JunctionRoutes : IRoutes
    {
        private readonly LaneJunction _junction;

        public JunctionRoutes( LaneJunction junction )
        {
            this._junction = junction;
        }

        public IEnumerable<RouteElement> GetRandomRoute( Random rng, RouteElement oneBeforeLast )
        {
            var edge = this._junction.Edges.FirstOrDefault( e => e.ConnectedEdge == oneBeforeLast.RoadElement );
            if ( edge == null ) { throw new ArgumentException( "Not connected element" ); }

            return edge.Routes.GetRandomRoute( rng, oneBeforeLast );
        }

        public void CalculateProbabilities()
        {
            this._junction.Top.Routes.CalculateProbabilities();
            this._junction.Right.Routes.CalculateProbabilities();
            this._junction.Bottom.Routes.CalculateProbabilities();
            this._junction.Left.Routes.CalculateProbabilities();
        }

        public void Add( BuildRoute route )
        {
            throw new InvalidOperationException( "Use route from edge" );
        }
    }
}
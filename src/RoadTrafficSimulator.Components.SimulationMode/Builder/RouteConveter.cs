using System;
using System.Linq;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public class RouteConveter
    {
        private readonly Func<IControl, IRoadElement> _roadElementsResolver;

        public RouteConveter( Func<IControl, IRoadElement> roadElementsResolver )
        {
            this._roadElementsResolver = roadElementsResolver;
        }

        public Routes Convert( BuildMode.Controls.Routes buildRoutes )
        {
            var route = buildRoutes.AvailableRoutes.Select( this.Convert );
            var routes = new Routes( route );
            routes.CalculateProbabilities();

            return routes;
        }

        private BuildRoute Convert( BuildMode.Controls.Route route )
        {
//            var controls = route.Select( s => this._roadElementsResolver( s ) );
//            return new Route( controls ) { Probability = route.Probability };
            throw new NotImplementedException();
        }
    }
}
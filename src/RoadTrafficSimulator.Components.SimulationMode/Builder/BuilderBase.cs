using System.Collections.Generic;
using System.Linq;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.SimulationMode.Route;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public class BuilderBase
    {
        private readonly BuildRoutesToSimulationRoutesConverter _converter = new BuildRoutesToSimulationRoutesConverter();

        protected virtual BuildRoute[] ConvertRoutes( Routes routes, BuilderContext obj, IRoadElement owner )
        {
            return this._converter.Convert( routes.AvailableRoutes, obj, owner ).ToArray();
        }

        protected virtual void SetConnections( IEnumerable<BuildRoute> convertedRoutes )
        {
            foreach ( var convertedRoute in convertedRoutes )
            {
                this.SetConnections( convertedRoute );
            }
        }

        private void SetConnections( BuildRoute convertedRoutes )
        {
            var mark = new Route<RouteElement>( convertedRoutes.Elements );

//            if ( !mark.MoveNext() ) { return; }
            while ( mark.MoveNext() )
            {
                mark.Current.RoadElement.Routes.AddRoadThatBelongToIt(convertedRoutes, mark.Clone() );
            }
        }
    }
}
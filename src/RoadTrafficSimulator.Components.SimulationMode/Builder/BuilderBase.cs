using System.Collections.Generic;
using System.Linq;
using RoadTrafficSimulator.Components.BuildMode.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public class BuilderBase
    {
        private readonly BuildRoutesToSimulationRoutesConverter _converter = new BuildRoutesToSimulationRoutesConverter();

        public virtual BuildRoute[] ConvertRoutes( Routes routes, BuilderContext obj )
        {
            return this._converter.Convert( routes.AvailableRoutes, obj ).ToArray();
        }

        public virtual void SetConnections( IEnumerable<BuildRoute> convertedRoutes, IRoadElement routeOwner )
        {
            foreach ( var convertedRoute in convertedRoutes )
            {
                this.SetConnections( convertedRoute, routeOwner );
            }
        }

        private void SetConnections( BuildRoute convertedRoutes, IRoadElement routeOwner )
        {
            var routes = convertedRoutes.Elements.ToArray();
            IRoadElement previous = routeOwner;
            var next = routes.Length >= 2 ? routes[ 1 ].RoadElement : null;

            for ( var i = 0; i < routes.Length; i++ )
            {
                routes[ i ].RoadElement.RoadInformation.SetConnection( next );
                routes[ i ].RoadElement.RoadInformation.SetReversConnection( previous );

                previous = routes[ i ].RoadElement;
                next = routes.Length > i + 1 ? routes[ i + 1 ].RoadElement : null;
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using RoadTrafficSimulator.Components.SimulationMode.Builder;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors.Infrastructure
{
    public class RouteToConductorConverter
    {
        private readonly ConductorResolver _conductorResolver;

        public RouteToConductorConverter( ConductorResolver conductorResolver )
        {
            this._conductorResolver = conductorResolver;
        }

        public IEnumerable<IConductor> Convert( IEnumerable<RouteElement> route )
        {
            var routes = route.ToArray();
            for ( var i = 0; i < routes.Length; i++ )
            {
                var previous = i - 1 >= 0 ? routes[ i - 1 ] : RouteElement.Empty;
                var next = i + 1 < routes.Length ? routes[ i + 1 ] : RouteElement.Empty;

                yield return this.Convert( routes[ i ], previous, next );
            }
        }

        private IConductor Convert( RouteElement routeElement, RouteElement previous, RouteElement next )
        {
            var condcutor = this._conductorResolver.Resolve( routeElement.RoadElement.GetType() ); // TODO Remove reflection
            condcutor.Setup( routeElement, routeElement.CanStopOnIt, previous.RoadElement, next.RoadElement, routeElement.PriorityType );// TODO This is awful
            return condcutor;
        }
    }
}
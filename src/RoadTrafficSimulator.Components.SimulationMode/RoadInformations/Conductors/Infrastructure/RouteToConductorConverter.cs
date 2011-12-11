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
            return route.Select( t => t == null ? ( IConductor ) null : Convert( ( RouteElement ) t ) );
        }

        public IConductor Convert( RouteElement routeElement )
        {
            var condcutor = this._conductorResolver.Resolve( routeElement.RoadElement.GetType(), routeElement.PriorityType ); // TODO Remove reflection
            condcutor.SetRouteElement( routeElement.RoadElement ); // TODO This is awful
            condcutor.SetCanStopOnIt( routeElement.CanStopOnIt ); // TODO This is awful
            return condcutor;
        }
    }
}
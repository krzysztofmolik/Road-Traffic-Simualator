using System.Collections.Generic;
using System.Linq;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public class BuildRoutesToSimulationRoutesConverter
    {
        public IEnumerable<BuildRoute> Convert( IEnumerable<BuildMode.Controls.Route> buildRoutes, BuilderContext context )
        {
            return buildRoutes.Select( route => this.ConvertRoute( route, context ) );
        }

        private BuildRoute ConvertRoute( BuildMode.Controls.Route route, BuilderContext context )
        {
            return new BuildRoute( this.GetRouteElements( route, context ) )
                       {
                           Probability = route.Probability,
                           Name = route.Name,
                       };
        }

        private IEnumerable<RouteElement> GetRouteElements( BuildMode.Controls.Route route, BuilderContext context )
        {
            return route.Items.Select( r => new RouteElement { PriorityType = r.PriorityType, RoadElement = this.GetRoadElement( context, r.Control ) } );
        }

        private IRoadElement GetRoadElement( BuilderContext context, IControl control )
        {
            if ( control is RoadJunctionEdge )
            {
                var edge = ( RoadJunctionEdge ) control;
                var laneJunction = context.GetObject<LaneJunction>( edge.Parent );
                return laneJunction.Edges.ElementAt( edge.EdgeIndex );
            }

            return context.GetObject<IRoadElement>( control );
        }
    }
}
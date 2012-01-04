using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;
using Common;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public class BuildRoutesToSimulationRoutesConverter
    {
        public IEnumerable<BuildRoute> Convert( IEnumerable<BuildMode.Controls.Route> buildRoutes, BuilderContext context, IRoadElement routeOwner )
        {
            return buildRoutes.Select( route => this.ConvertRoute( route, context, routeOwner ) );
        }

        private BuildRoute ConvertRoute( BuildMode.Controls.Route route, BuilderContext context, IRoadElement routeOwner )
        {
            return new BuildRoute( this.GetRouteElements( route, context, routeOwner ) )
                       {
                           Probability = route.Probability,
                           Name = route.Name,
                           Owner = routeOwner,
                       };
        }

        private IEnumerable<RouteElement> GetRouteElements( BuildMode.Controls.Route route, BuilderContext context, IRoadElement routeOwner )
        {
            return route.Items.Select( ( index, prev, current, next ) =>
                                           {
                                               var nextElement = next != null ? next.Control : null;
                                               var prevElement = prev != null ? prev.Control : null;
                                               if ( prevElement == null && index == 0 )
                                               {
                                                   prevElement = routeOwner.BuildControl;
                                               }

                                               return new RouteElement
                                                          {
                                                              PriorityType = current.PriorityType,
                                                              RoadElement = this.GetRoadElement( context, current.Control ),
                                                              Length = this.GetLength( prevElement, current, nextElement ),
                                                              CanStopOnIt = current.CanStop,
                                                          };
                                           } );
        }

        // TODO zmienic na cos bardziej sensownego
        private float GetLength( IControl prev, BuildMode.Controls.RouteElement current, IControl next )
        {
            if ( current.Control is RoadLaneBlock )
            {
                var lane = ( RoadLaneBlock ) current.Control;
                return Vector2.Distance( lane.LeftEdge.Location, lane.RightEdge.Location );
            }
            if ( current.Control is RoadJunctionBlock )
            {
                return Vector2.Distance( prev.Location, next.Location );
            }

            return Constans.PointSize;
        }

        private IRoadElement GetRoadElement( BuilderContext context, IControl control )
        {
            return context.GetObject<IRoadElement>( control );
        }
    }
}
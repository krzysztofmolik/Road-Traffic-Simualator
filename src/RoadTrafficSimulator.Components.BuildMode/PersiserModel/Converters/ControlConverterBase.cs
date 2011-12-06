using System;
using System.Collections.Generic;
using System.Linq;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Converters
{
    public abstract class ControlConverterBase : IControlConverter
    {
        protected virtual IAction BuildRoutes<TControl>( TControl control ) where TControl : IRoadElement, IControl
        {
            var routesAddActions = control.Routes.AvailableRoutes
                .Select( route =>
                             {
                                 var routesActions = this.BuildSingleRoute( route );
                                 return Actions.Call<TControl>( control.Id, () => control.Routes.AddRoute( Is.Action<Route>( routesActions ) ) );
                             } )
                .ToArray();
            return new ActionCollection( Order.Low ).AddRange( routesAddActions );
        }

        protected virtual IAction BuildSingleRoute( Route route )
        {
            var createRouteAction = Actions.Ctor( () => new Route( Is.Const( route.Name ), Is.Const( route.Probability ) ) );
            var addElementActions = route.Items.Select( routeElement =>
                                                        Actions.Call( () => createRouteAction.Default.Add( Is.Control( routeElement.Control ), Is.Const( routeElement.PriorityType ) ) ) )
                .ToArray();

            return new ActionCollection( Order.Low ).Add( createRouteAction ).AddRange( addElementActions ).Return( createRouteAction );
        }

        public abstract Type Type { get; }
        public abstract IEnumerable<IAction> ConvertToAction( IControl control );
    }
}
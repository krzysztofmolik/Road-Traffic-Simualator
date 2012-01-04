using System.Collections.Generic;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.ConnectObject
{
    public class RouteConveter
    {
        private readonly PriorityTypeFactory _priorityFactory;
        private readonly ControlToControlViewModelConveter _controlToControlViewModelConveter;


        public RouteConveter()
            : this( new ControlToControlViewModelConveter())
        { }

        public RouteConveter( ControlToControlViewModelConveter controlToControlViewModelConveter )
        {
            this._priorityFactory = new PriorityTypeFactory();
            this._controlToControlViewModelConveter = controlToControlViewModelConveter;
        }

        public IEnumerable<RouteViewModel> Conveter( IControl control )
        {
            var roadElement = control as IRouteOwner;
            if ( roadElement == null ) { yield break; }

            foreach ( var route in roadElement.Routes.AvailableRoutes )
            {
                yield return this.Conveter( control, route );
            }
        }

        public RouteItemViewModel Convert( RouteElement routeElement )
        {
            return new RouteItemViewModel( this._controlToControlViewModelConveter.Convert( routeElement.Control ),
                                           this._priorityFactory.PossiblePriorityTypes( null, null ), routeElement )
                       {
                           CanStopOnIt = routeElement.CanStop,
                           Priority = routeElement.PriorityType,
                       };
        }

        private RouteViewModel Conveter( IControl control, Route route )
        {
            var resutl = new RouteViewModel( route );
            var previousControl = this._controlToControlViewModelConveter.Convert( control );
            foreach ( var routeElement in route.Items )
            {
                var item = this.Convert( routeElement );
                resutl.Add( item );
            }
            return resutl;
        }
    }
}
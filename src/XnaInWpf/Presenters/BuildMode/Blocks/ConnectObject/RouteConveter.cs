using System.Collections.Generic;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.ConnectObject
{
    public class RouteConveter
    {
        private readonly PriorityTypeFactory _priorityFactory;
        private readonly ControlToControlViewModelConveter _controlToControlViewModelConveter;

        public RouteConveter( ControlToControlViewModelConveter controlToControlViewModelConveter )
        {
            this._priorityFactory = new PriorityTypeFactory();
            this._controlToControlViewModelConveter = controlToControlViewModelConveter;
        }

        public IEnumerable<RouteViewModel> Conveter( IControl control )
        {
            var roadElement = control as IRoadElement;
            if ( roadElement == null ) { yield break; }

            foreach ( var route in roadElement.Routes.AvailableRoutes )
            {
                yield return this.Conveter( control, route );
            }
        }

        private RouteViewModel Conveter( IControl control, Route route )
        {
            var resutl = new RouteViewModel();
            var previousControl = this._controlToControlViewModelConveter.Convert( control );
            foreach ( var routeElement in route.Items )
            {
                var currentControl = this._controlToControlViewModelConveter.Convert( routeElement.Control );
                var item = new RouteItemViewModel( currentControl, this._priorityFactory.PossiblePriorityTypes( previousControl.Control, routeElement.Control ) );
                resutl.Add( item );
            }
            return resutl;
        }
    }
}
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.ConnectObject
{
    public class ControlToControlWithRouteViewModelConveter
    {
        private readonly RouteConveter _routeConveter;

        public ControlToControlWithRouteViewModelConveter()
        {
            this._routeConveter = new RouteConveter( new ControlToControlViewModelConveter() );
        }

        public ControlWithRoutelViewModel Convert( IControl control )
        {
            return new ControlWithRoutelViewModel( control, this._routeConveter.Conveter( control ) );
        }
    }
}
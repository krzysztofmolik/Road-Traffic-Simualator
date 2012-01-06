using System.Collections.Generic;
using RoadTrafficConstructor.Presenters.BuildMode.Blocks.Editors;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Controls;
using Common;
using System.Linq;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.ConnectObject
{
    public class ControlToControlWithRouteViewModelConveter
    {
        private readonly RouteConveter _routeConveter;

        public ControlToControlWithRouteViewModelConveter()
        {
            this._routeConveter = new RouteConveter( new ControlToControlViewModelConveter() );
        }

        public IControlWithRoutelViewModel Convert( IControl control )
        {
            // TODO Create some kind of handlers insted of if's
            if ( control is RoadTrafficSimulator.Components.BuildMode.Controls.CarsInserter )
            {
                return new CarInserterEditorViewModel( ( RoadTrafficSimulator.Components.BuildMode.Controls.CarsInserter ) control, this._routeConveter.Conveter( control ) );
            }

            if ( control is LightBlock )
            {
                return new LightEditorViewModel( ( LightBlock ) control );
            }

            return new DefaultControlEditorViewModel( control, this._routeConveter.Conveter( control ) );
        }

        public IEnumerable<IControlWithRoutelViewModel> Convert( IEnumerable<IControl> controls )
        {
            return controls.Select( this.Convert );
        }
    }
}
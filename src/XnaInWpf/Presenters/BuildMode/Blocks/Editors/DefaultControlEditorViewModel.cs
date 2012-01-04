using System;
using System.Collections.Generic;
using System.ComponentModel;
using RoadTrafficConstructor.Presenters.BuildMode.Blocks.ConnectObject;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.Editors
{
    public class DefaultControlEditorViewModel : IControlWithRoutelViewModel
    {
        private readonly BasiInformationAboutControlViewModel _basicInformation;
        private readonly RouteEditorViewModel _routeEditor;
        private readonly IRouteOwner _control;

        public DefaultControlEditorViewModel( IControl control, IEnumerable<RouteViewModel> routes )
        {
            if ( !( control is IRouteOwner ) ) { throw new ArgumentException(); }
            this._control = ( IRouteOwner) control;
            this._basicInformation = new BasiInformationAboutControlViewModel( control );
            this._routeEditor = new RouteEditorViewModel( this._control, routes );
        }

        public BasiInformationAboutControlViewModel BasicInformation
        {
            get { return this._basicInformation; }
        }

        public RouteEditorViewModel RouteEditor
        {
            get { return this._routeEditor; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void ControlClicked( ControlViewModel control )
        {
            this.RouteEditor.ControlCliced( control );
        }
    }
}
using System.Collections.Generic;
using System.ComponentModel;
using RoadTrafficConstructor.Presenters.BuildMode.Blocks.ConnectObject;
using Common.Wpf;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.Editors
{
    public class CarInserterEditorViewModel : IControlWithRoutelViewModel
    {
        private readonly BasiInformationAboutControlViewModel _basicInformation;
        private readonly RouteEditorViewModel _routeEditor;
        private readonly RoadTrafficSimulator.Components.BuildMode.Controls.CarsInserter _carInserter;

        public CarInserterEditorViewModel( RoadTrafficSimulator.Components.BuildMode.Controls.CarsInserter control, IEnumerable<RouteViewModel> routes )
        {
            this._carInserter = control;
            this._basicInformation = new BasiInformationAboutControlViewModel( control );
            this._routeEditor = new RouteEditorViewModel( this._carInserter, routes );

            this.CarInsertInterval = this._carInserter.GetCarInsertInterval();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void ControlClicked( ControlViewModel control )
        {
            this.RouteEditor.ControlCliced( control );
        }

        public BasiInformationAboutControlViewModel BasicInformation
        {
            get { return this._basicInformation; }
        }

        public RouteEditorViewModel RouteEditor
        {
            get { return this._routeEditor; }
        }

        private uint _carInsertInterval;
        public uint CarInsertInterval
        {
            get { return this._carInsertInterval; }
            set
            {
                this._carInsertInterval = value;
                this._carInserter.SetCarInsertInterval( value );
                this.PropertyChanged.Raise( this, () => this.CarInsertInterval );
            }
        }
    }
}

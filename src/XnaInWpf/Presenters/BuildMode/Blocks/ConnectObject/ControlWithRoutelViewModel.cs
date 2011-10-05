using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.ConnectObject
{
    public class ControlWithRoutelViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly IControl _control;
        private readonly string _controlType;
        private readonly ObservableCollection<RouteViewModel> _route;

        public ControlWithRoutelViewModel( IControl control, IEnumerable<RouteViewModel> route )
        {
            this._control = control;
            this._controlType = this._control.GetType().Name;
            this._route = new ObservableCollection<RouteViewModel>( route );
        }

        public string ControlType
        {
            get { return this._controlType; }
        }

        public ObservableCollection<RouteViewModel> Route
        {
            get { return this._route; }
        }

        public IControl Control
        {
            get { return this._control; }
        }
    }
}
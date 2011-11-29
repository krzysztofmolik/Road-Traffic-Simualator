using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Controls;
using Common.Wpf;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.ConnectObject
{
    public class ControlWithRoutelViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly IControl _control;
        private readonly string _controlType;
        private readonly ObservableCollection<RouteViewModel> _routes;

        public ControlWithRoutelViewModel(IControl control, IEnumerable<RouteViewModel> route)
        {
            this._control = control;
            this._controlType = this._control.GetType().Name;
            this._routes = new ObservableCollection<RouteViewModel>(route);
        }

        public string ControlType
        {
            get { return this._controlType; }
        }

        public Vector2 Location
        {
            get { return this._control.Location; }
        }

        public ObservableCollection<RouteViewModel> Routes
        {
            get { return this._routes; }
        }

        private RouteViewModel _selectedRoad;
        public RouteViewModel SelectedRoad
        {
            get { return this._selectedRoad; }
            set
            {
                this._selectedRoad = value;
                this.PropertyChanged.Raise(this, () => this.SelectedRoad);
            }
        }

        public IControl Control
        {
            get { return this._control; }
        }

        public void ControlClicked(ControlViewModel control)
        {
            if( this.SelectedRoad == null ) { return; }

            this.SelectedRoad.ControlClicked(control);
        }
    }
}
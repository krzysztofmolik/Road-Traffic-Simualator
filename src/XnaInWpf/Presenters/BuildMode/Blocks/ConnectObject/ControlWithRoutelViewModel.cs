using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using NLog;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Controls;
using Common.Wpf;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.ConnectObject
{
    public class ControlWithRoutelViewModel : INotifyPropertyChanged
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly IControl _control;
        private readonly string _controlType;
        private readonly ObservableCollection<RouteViewModel> _routes;

        public ControlWithRoutelViewModel( IControl control, IEnumerable<RouteViewModel> route )
        {
            this._control = control;
            this._controlType = this._control.GetType().Name;
            this._routes = new ObservableCollection<RouteViewModel>( route );
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
                if ( this._selectedRoad != null ) { this._selectedRoad.IsSelected = false; }
                this._selectedRoad = value;
                if ( this._selectedRoad != null ) { this._selectedRoad.IsSelected = true; }
                this.PropertyChanged.Raise( this, () => this.SelectedRoad );
            }
        }

        public IControl Control
        {
            get { return this._control; }
        }

        public void ControlClicked( ControlViewModel control )
        {
            if ( this.SelectedRoad == null ) { return; }

            this.SelectedRoad.ControlClicked( control );
        }

        public void RemoveSelected()
        {
            if ( this._selectedRoad == null ) { Logger.Info( "Nie ma co usuwac" ); return; }
            var toRemove = this._selectedRoad;
            this._selectedRoad = null;
            this._routes.Remove( toRemove );
            var routeElement = this.Control as IRoadElement;
            if ( routeElement == null )
            {
                Logger.Warn( "Cos sie sp*****" );
                return;
            }

            routeElement.Routes.Remove( toRemove.OrginalRoute );
        }

        public void AddNewRoute()
        {
            var routeElement = this.Control as IRoadElement;
            if ( routeElement == null )
            {
                Logger.Warn( "Cos sie sp*****" );
                return;
            }

            // NOTE Yeaaa do everything in GUI ;)
            var route = new Route( "Unknow", 0.0f );
            routeElement.Routes.AddRoute( route );
            var routeViewModel = new RouteViewModel( route.Name, route );
            this._routes.Add( routeViewModel );
            this.SelectedRoad = routeViewModel;
        }
    }
}
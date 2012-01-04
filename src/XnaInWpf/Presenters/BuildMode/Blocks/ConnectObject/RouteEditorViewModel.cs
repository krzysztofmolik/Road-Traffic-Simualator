using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Common.Wpf;
using NLog;
using RoadTrafficSimulator.Components.BuildMode.Controls;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.ConnectObject
{
    public class RouteEditorViewModel : INotifyPropertyChanged
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ObservableCollection<RouteViewModel> _routes;
        private readonly IRouteOwner _owner;

        public RouteEditorViewModel( IRouteOwner owner, IEnumerable<RouteViewModel> route )
        {
            this._owner = owner;
            this._routes = new ObservableCollection<RouteViewModel>( route );
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RemoveSelected()
        {
            if ( this._selectedRoad == null ) { Logger.Info( "Nie ma co usuwac" ); return; }
            var toRemove = this._selectedRoad;
            this._selectedRoad = null;
            this._routes.Remove( toRemove );

            this._owner.Routes.Remove( toRemove.OrginalRoute );
        }

        public void AddNewRoute()
        {
            var route = new Route( "Unknow", 100, this._owner );
            this._owner.Routes.AddRoute( route );
            var routeViewModel = new RouteViewModel( route );
            this._routes.Add( routeViewModel );
            this.SelectedRoad = routeViewModel;
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

        public void ControlCliced( ControlViewModel control )
        {
            if ( this.SelectedRoad == null ) { return; }

            this.SelectedRoad.ControlClicked( control );
        }
    }
}
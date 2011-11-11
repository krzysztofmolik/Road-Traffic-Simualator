using System.Collections.ObjectModel;
using System.ComponentModel;
using Common.Wpf;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.ConnectObject
{
    public class RouteViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<RouteItemViewModel> _items;

        public RouteViewModel()
        {
            this._items = new ObservableCollection<RouteItemViewModel>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;
        public string Name
        {
            get { return this._name; }
            set { this._name = value; this.PropertyChanged.Raise( this, () => this.Name ); }
        }

        public void Add( RouteItemViewModel item )
        {
            this._items.Add( item );
        }

        public ObservableCollection<RouteItemViewModel> Items
        {
            get { return this._items; }
        }
    }
}
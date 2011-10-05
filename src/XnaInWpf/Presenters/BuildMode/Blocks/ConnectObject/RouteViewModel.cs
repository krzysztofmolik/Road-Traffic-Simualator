using System.Collections.ObjectModel;
using System.ComponentModel;

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
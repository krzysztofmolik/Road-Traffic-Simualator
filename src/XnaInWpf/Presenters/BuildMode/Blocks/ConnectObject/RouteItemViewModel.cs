using System.Collections.Generic;
using System.Collections.ObjectModel;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.ConnectObject
{
    public class RouteItemViewModel
    {
        private readonly ObservableCollection<PriorityType> _priorityTypes;
        private readonly ControlViewModel _controlWithRoutel;

        public RouteItemViewModel( ControlViewModel controlWithRoutel, IEnumerable<PriorityType> priorityTypes )
        {
            this._priorityTypes = new ObservableCollection<PriorityType>( priorityTypes );
            this._controlWithRoutel = controlWithRoutel;
        }

        public ObservableCollection<PriorityType> PriorityTypes
        {
            get { return this._priorityTypes; }
        }

        public ControlViewModel ControlWithRoutel
        {
            get { return this._controlWithRoutel; }
        }
    }
}
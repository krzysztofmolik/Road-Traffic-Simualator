using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Controls;
using Common.Wpf;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.ConnectObject
{
    public class RouteItemViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<PriorityType> _priorityTypes;
        private readonly ControlViewModel _control;
        private readonly RouteElement _orginalRouteElement;

        public event PropertyChangedEventHandler PropertyChanged;

        public RouteItemViewModel( ControlViewModel control, IEnumerable<PriorityType> priorityTypes, RouteElement orginalRouteElement )
        {
            this._priorityTypes = new ObservableCollection<PriorityType>( priorityTypes );
            this._control = control;
            this._orginalRouteElement = orginalRouteElement;
            this.CanStopOnIt = orginalRouteElement.CanStop;
        }

        public ObservableCollection<PriorityType> PriorityTypes
        {
            get { return this._priorityTypes; }
        }

        private PriorityType _priority;
        public PriorityType Priority
        {
            get { return this._priority; }
            set
            {
                this._priority = value;
                this._orginalRouteElement.PriorityType = value;
                this.PropertyChanged.Raise( this, () => this.Priority );
            }
        }

        private bool _canStopOnIt;
        public bool CanStopOnIt
        {
            get { return this._canStopOnIt; }
            set
            {
                this._canStopOnIt = value;
                this._orginalRouteElement.CanStop = value;
                this.PropertyChanged.Raise( this, () => this.CanStopOnIt );
            }
        }

        public ControlViewModel Control
        {
            get { return this._control; }
        }

        private bool _isSelectedOnSimulator;
        public bool IsSelectedOnSimulator
        {
            get { return this._isSelectedOnSimulator; }
            set
            {
                if ( this._isSelectedOnSimulator == value ) { return; }
                this._isSelectedOnSimulator = value;
                if ( this._isSelectedOnSimulator )
                {
                    this._control.Control.VertexContainer.Color = Color.Red;
                }
                else
                {
                    this._control.Control.VertexContainer.ClearColor();
                }
                this._control.Control.Invalidate();
                this.PropertyChanged.Raise( this, () => this.IsSelectedOnSimulator );
            }
        }
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Controls;
using Common.Wpf;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.ConnectObject
{
    public class RouteItemViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<PriorityType> _priorityTypes;
        private readonly ControlViewModel _control;

        public event PropertyChangedEventHandler PropertyChanged;

        public RouteItemViewModel( ControlViewModel control, IEnumerable<PriorityType> priorityTypes )
        {
            this._priorityTypes = new ObservableCollection<PriorityType>( priorityTypes );
            this._control = control;
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
                this.PropertyChanged.Raise( this, () => this.Priority );
            }
        }

        public ControlViewModel Control
        {
            get { return this._control; }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return this._isSelected; }
            set
            {
                this._isSelected = value;
                if ( this._isSelected )
                {
                    this._control.Control.VertexContainer.Color = Color.Red;
                }
                else
                {
                    this._control.Control.VertexContainer.ClearColor();
                }
                this._control.Control.Invalidate();
                this.PropertyChanged.Raise( this, () => this.IsSelected );
            }
        }
    }
}
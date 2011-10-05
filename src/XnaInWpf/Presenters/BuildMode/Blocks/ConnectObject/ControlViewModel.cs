using System.ComponentModel;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.ConnectObject
{
    public class ControlViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly IControl _control;
        private readonly string _controlType;

        public ControlViewModel( IControl control )
        {
            this._control = control;
            this._controlType = this._control.GetType().Name;
        }

        public string ControlType
        {
            get { return this._controlType; }
        }

        public IControl Control
        {
            get { return this._control; }
        }
    }
}
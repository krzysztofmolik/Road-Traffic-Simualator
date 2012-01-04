using System.ComponentModel;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.ConnectObject
{
    public interface IControlWithRoutelViewModel : INotifyPropertyChanged
    {
        void ControlClicked( ControlViewModel control );
    }
}
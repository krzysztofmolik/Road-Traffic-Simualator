using System.ComponentModel;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficConstructor.Presenters.BottomTabs.Settings
{
    public interface ISettingViewMode : INotifyPropertyChanged
    {
        bool SupportSetting( IControl control );
        void SetStting( IControl roadJunctionBlock );
    }
}
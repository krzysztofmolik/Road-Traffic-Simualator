using System.ComponentModel;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficConstructor.Presenters.BottomTabs.Settings
{
    public class DefaultSettingViewModel : ISettingViewMode
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool SupportSetting( IControl control )
        {
            return true;
        }

        public void SetStting( IControl roadJunctionBlock )
        {
        }
    }
}
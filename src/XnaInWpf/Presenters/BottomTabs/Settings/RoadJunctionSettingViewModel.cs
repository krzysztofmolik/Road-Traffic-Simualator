using System.ComponentModel;
using Common.Wpf;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficConstructor.Presenters.BottomTabs.Settings
{
    public class RoadJunctionSettingViewModel : ISettingViewMode
    {
        private RoadJunctionBlock _control;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool SupportSetting( IControl control )
        {
            return control is RoadJunctionBlock;
        }

        public void SetStting( IControl roadJunctionBlock )
        {
        }

    }
}
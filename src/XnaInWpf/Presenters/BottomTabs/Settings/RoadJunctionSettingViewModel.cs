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
            this._control = ( RoadJunctionBlock ) roadJunctionBlock;
            this.PropertyChanged.Raise( this, () => this.CanLeftBeSet );
            this.PropertyChanged.Raise( this, () => this.IsLeftOut );
            this.PropertyChanged.Raise( this, () => this.CanTopBeSet );
            this.PropertyChanged.Raise( this, () => this.IsTopOut );
            this.PropertyChanged.Raise( this, () => this.CanRightBeSet );
            this.PropertyChanged.Raise( this, () => this.IsRightOut );
            this.PropertyChanged.Raise( this, () => this.CanBottomBeSet );
            this.PropertyChanged.Raise( this, () => this.IsBottomOut );
        }

        public bool CanLeftBeSet
        {
            get { return this._control.LeftEdge.CanChangeIsOut; }
        }

        public bool IsLeftOut
        {
            get { return this._control.LeftEdge.IsOut; }
            set
            {
                this._control.LeftEdge.IsOut = value;
                this.PropertyChanged.Raise( this, () => this.IsLeftOut );
            }
        }

        public bool CanTopBeSet
        {
            get { return this._control.TopEdge.CanChangeIsOut; }
        }

        public bool IsTopOut
        {
            get { return this._control.TopEdge.IsOut; }
            set
            {
                this._control.TopEdge.IsOut = value;
                this.PropertyChanged.Raise( this, () => this.IsTopOut );
            }
        }

        public bool CanRightBeSet
        {
            get { return this._control.RightEdge.CanChangeIsOut; }
        }

        public bool IsRightOut
        {
            get { return this._control.RightEdge.IsOut; }
            set
            {
                this._control.RightEdge.IsOut = value;
                this.PropertyChanged.Raise( this, () => this.IsRightOut );
            }
        }

        public bool CanBottomBeSet
        {
            get { return this._control.BottomEdge.CanChangeIsOut; }
        }

        public bool IsBottomOut
        {
            get { return this._control.BottomEdge.IsOut; }
            set
            {
                this._control.BottomEdge.IsOut = value;
                this.PropertyChanged.Raise( this, () => this.IsBottomOut );
            }
        }
    }
}
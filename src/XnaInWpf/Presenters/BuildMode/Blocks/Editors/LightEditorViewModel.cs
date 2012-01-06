using System;
using System.ComponentModel;
using Common.Wpf;
using RoadTrafficConstructor.Presenters.BuildMode.Blocks.ConnectObject;
using RoadTrafficSimulator.Components.BuildMode.Controls;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.Editors
{
    public class LightEditorViewModel : IControlWithRoutelViewModel
    {
        private readonly BasiInformationAboutControlViewModel _basicInformation;
        private readonly LightBlock _light;

        public LightEditorViewModel( LightBlock control )
        {
            this._light = control;
            this._basicInformation = new BasiInformationAboutControlViewModel( control );

            this.SetupDealy = this._light.Times.SetupDealy.Seconds;
            this.RedLightTime = this._light.Times.RedLightTime.Seconds;
            this.YellowLightTime = this._light.Times.YellowLightTime.Seconds;
            this.GreenLightTime = this._light.Times.GreenLightTime.Seconds;
        }

        private int _setupDealy;
        public int SetupDealy
        {
            get { return this._setupDealy; }
            set
            {
                this._setupDealy = value;
                this._light.Times.SetupDealy = TimeSpan.FromSeconds( value );
                this.PropertyChanged.Raise( this, () => this.SetupDealy );
            }
        }

        private int _redLightTime;
        public int RedLightTime
        {
            get { return this._redLightTime; }
            set
            {
                this._redLightTime = value;
                this._light.Times.RedLightTime = TimeSpan.FromSeconds( value );
                this.PropertyChanged.Raise( this, () => this.RedLightTime );
            }
        }

        private int _yellowLightTime;
        public int YellowLightTime
        {
            get { return this._yellowLightTime; }
            set
            {
                this._yellowLightTime = value;
                this._light.Times.YellowLightTime = TimeSpan.FromSeconds( value );
                this.PropertyChanged.Raise( this, () => this.YellowLightTime );
            }
        }

        private int _greenLightTime;
        public int GreenLightTime
        {
            get { return this._greenLightTime; }
            set
            {
                this._greenLightTime = value;
                this._light.Times.GreenLightTime = TimeSpan.FromSeconds( value );
                this.PropertyChanged.Raise( this, () => this.GreenLightTime );
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void ControlClicked( ControlViewModel control )
        {
        }

        public BasiInformationAboutControlViewModel BasicInformation
        {
            get { return this._basicInformation; }
        }
    }
}
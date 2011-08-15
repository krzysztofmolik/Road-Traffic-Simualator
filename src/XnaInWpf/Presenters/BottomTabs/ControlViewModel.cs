using System;
using System.ComponentModel;
using System.IO;
using Common;
using Common.Wpf;
using RoadTrafficSimulator.Components.BuildMode.Messages;
using RoadTrafficSimulator.Infrastructure.Messages;

namespace RoadTrafficConstructor.Presenters.BottomTabs
{
    public class ControlViewModel : INotifyPropertyChanged
    {
        private readonly IEventAggregator _eventAggregator;

        public ControlViewModel( IEventAggregator eventAggregator )
        {
            this._eventAggregator = eventAggregator;
            this.ZoomValue = 50;
            this.SwitchToBuildMode();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isBuildMode;
        public bool IsBuildMode
        {
            get { return this._isBuildMode; }
            set
            {
                this._isBuildMode = value;
                this.PropertyChanged.Raise( this, () => this.IsBuildMode );
            }
        }

        public void SwtichToSimulationMode()
        {
            this.IsBuildMode = false;
            this._eventAggregator.Publish( new ChangedToSimulationMode() );
        }

        public void SwitchToBuildMode()
        {
            this.IsBuildMode = true;
            this._eventAggregator.Publish( new ChangedToBuildMode() );
        }

        public void Save()
        {
            var fileName = Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ) + @"\1.traffic";
            if ( File.Exists( fileName ) ) { File.Delete( fileName ); }
            var stream = File.Open( fileName, FileMode.OpenOrCreate, FileAccess.Write );
            this._eventAggregator.Publish( new SaveMap( stream ) );
        }

        public void Load()
        {
            var fileName = Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments ) + @"\1.traffic";
            if ( !File.Exists( fileName ) ) { throw new ArgumentException( "fileName" ); }
            var stream = File.Open( fileName, FileMode.Open );
            this._eventAggregator.Publish( new LoadMap( stream ) );
        }

        public void IncreaseZoom()
        {
            this.ZoomValue += 10;
        }

        public void DecreaseZoom()
        {
            this.ZoomValue -= 10;
        }

        private int _zoomValue;
        public int ZoomValue
        {
            get { return this._zoomValue; }
            set
            {
                var newValue = value;
                if ( newValue > 100 ) { newValue = 100; }
                if ( newValue < 0 ) { newValue = 0; }
                if ( newValue == this._zoomValue ) { return; }
                this._zoomValue = newValue;
                this.PropertyChanged.Raise( this, () => this.ZoomValue );
                this._eventAggregator.Publish( new ChangedZoom( newValue ) );
            }
        }
    }
}
using System;
using System.Linq;
using RoadTrafficSimulator.Infrastructure.Mouse;
using XnaInWpf.Presenters;

namespace RoadTrafficConstructor.Presenters
{
    public interface IMouseInformationModel
    {
        bool ShouldShowMousePositoion { get; set; }
        string MouseXPosition { get; set; }
        string MouseYPosition { get; set; }
    }

    public class DebugMouseInformationModel : MyModelBase, IMouseInformationModel
    {
        private readonly IMouseInformation _mouseInformation;
        private string _mouseXPosition;
        private string _mouseYPosition;
        private bool _shouldShowMousePosition;

        public DebugMouseInformationModel( IMouseInformation mouseInformation )
        {
            this._mouseInformation = mouseInformation;
            this._mouseInformation.MousePositionChanged.Where( t => this.ShouldShowMousePositoion).Subscribe(t =>
                                                                       {
                                                                           this.MouseXPosition = this._mouseInformation.XnaXMousePosition.ToString();
                                                                           this.MouseYPosition = this._mouseInformation.XnaYMousePosition.ToString();
                                                                       });
            this._mouseInformation.StartRecord();
        }

        public bool ShouldShowMousePositoion
        {
            get { return _shouldShowMousePosition; }
            set
            {
                this._shouldShowMousePosition = value;
                this.OnPropertyChanged( () => this.ShouldShowMousePositoion);
            }
        }

        public string MouseXPosition
        {
            get
            {
                return _mouseXPosition;
            }
            set
            {
                this._mouseXPosition = value;
                this.OnPropertyChanged( () => this.MouseXPosition );
            }
        }

        public string MouseYPosition
        {
            get { return this._mouseYPosition; }
            set 
            {
                this._mouseYPosition = value; 
                this.OnPropertyChanged( () => this.MouseYPosition );
            }
        }
    }
}
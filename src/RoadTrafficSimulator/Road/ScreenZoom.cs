using System;
using System.Linq;
using Common;
using Microsoft.Xna.Framework.Input;
using NLog;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.MouseHandler.Infrastructure;
using RoadTrafficSimulator.Utils;

namespace RoadTrafficSimulator.Road
{
    public interface IBackgroundJob
    {
        void Start();

        void Stop();
    }

    public class ScreenZoom : IBackgroundJob
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly KeyboardInputNotify _keyboard;
        private readonly IMouseInformation _mouseInforamtion;
        private readonly Camera3D _camera3D;
        private IDisposable _leftControlPressed;
        private IDisposable _leftControlReleased;
        private IDisposable _scrollChanged;

        public ScreenZoom( IMouseInformation mouseInformation, KeyboardInputNotify keyboard, Camera3D camera3D )
        {
            this._keyboard = keyboard;
            this._mouseInforamtion = mouseInformation.NotNull();
            this._camera3D = camera3D.NotNull();

        }

        public void Start()
        {
//            this._leftControlPressed = this._keyboard.KeyPressed.Where( s => s.Key == Keys.LeftControl ).Subscribe( s => this.BeginZooming() );
//            this._leftControlReleased = this._keyboard.KeyRelease.Where( s => s.Key == Keys.LeftControl ).Subscribe( s => this.EndZooming() );
            this._scrollChanged = this._mouseInforamtion.ScrollWheelChanged.Subscribe( this.Zooming );
        }

        public void Stop()
        {
            this._leftControlPressed.Dispose();
            this._leftControlReleased.Dispose();
            this._scrollChanged.Dispose();
        }

        private void EndZooming()
        {
            Logger.Trace("Stop recording mouse");
            this._mouseInforamtion.StopRecord();
        }

        private void BeginZooming()
        {
            Logger.Trace("Start recording mouse");
            this._mouseInforamtion.StartRecord();
        }

        private void Zooming( XnaMouseState mouseState )
        {
            this._camera3D.Zoom = mouseState.ScrollWheelValueDelta * 0.1f;
        }
    }
}
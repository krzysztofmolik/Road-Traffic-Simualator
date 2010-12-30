using System;
using System.Collections.Generic;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RoadTrafficSimulator.MouseHandler.Infrastructure;
using RoadTrafficSimulator.Utils;
using XnaVs10.Utils;

namespace RoadTrafficSimulator.Infrastructure.Mouse
{
    public class MouseInformation : IMouseInformation
    {
        private const float ClickDistance = 0.000001f;
        private readonly Timer _timer;
        private readonly Camera3D _camera;
        private readonly Subject<ButtonState> _lefButtonChanged;
        private readonly ISubject<XnaMouseState> _mousePositionChanged = new Subject<XnaMouseState>();
        private readonly ISubject<XnaMouseState> _leftButtonPressed = new Subject<XnaMouseState>();
        private readonly ISubject<XnaMouseState> _leftButtonReleased = new Subject<XnaMouseState>();
        private readonly ISubject<XnaMouseState> _leftButtonClicked = new Subject<XnaMouseState>();
        private readonly ISubject<XnaMouseState> _scrollWheelChanged = new Subject<XnaMouseState>();
        private Vector2 _xnaMousePosition;
        private Vector2 _screenMousePosition;
        private MouseState _previousMouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();

        private Vector2 _lastMoseDownPosition = Vector2.Zero;

        public MouseInformation( Camera3D camera )
        {
            this.MouseMoveDelta = Vector2.Zero;
            this._camera = camera;
            this._timer = new Timer { AutoReset = true };
            this._timer.Elapsed += this.UpdateMouseInformation;
            this._lefButtonChanged = new Subject<ButtonState>();
        }

        public int ScrollWheelValueDelta { get; private set; }

        public Vector2 MouseMoveDelta { get; private set; }

        public IObservable<XnaMouseState> MousePositionChanged { get { return this._mousePositionChanged; } }

        public IObservable<ButtonState> LeftButtonChanged { get { return this._lefButtonChanged; } }

        public IObservable<XnaMouseState> LeftButtonRelease
        {
            get { return this._leftButtonReleased; }
        }

        public IObservable<XnaMouseState> LeftButtonPressed
        {
            get { return this._leftButtonPressed; }
        }

        public IObservable<XnaMouseState> LeftButtonClicked
        {
            get { return this._leftButtonClicked; }
        }

        public IObservable<XnaMouseState> ScrollWheelChanged
        {
            get { return this._scrollWheelChanged; }
        }

        public Vector2 MouseXnaPosition
        {
            get { return new Vector2( this.XnaXMousePosition, this.XnaYMousePosition ); }
        }

        public float XnaXMousePosition
        {
            get
            {
                return this._xnaMousePosition.X;
            }
        }

        public float XnaYMousePosition
        {
            get { return this._xnaMousePosition.Y; }
        }

        public float ScreenXMousePosition
        {
            get { return this._screenMousePosition.X; }
        }

        public float ScreenYMousePosition
        {
            get { return this._screenMousePosition.Y; }
        }

        public void StartRecord( int inteval = 100 )
        {
            if( this._timer.Enabled )
            {
                return;
            }
            this._timer.Interval = inteval;
            this._timer.Start();
        }

        public void StopRecord()
        {
            this._timer.Stop();
        }

        public void Dispose()
        {
            this.StopRecord();
            this._mousePositionChanged.OnCompleted();
            this._timer.Elapsed -= UpdateMouseInformation;
        }

        private void UpdateMouseInformation( object sender, ElapsedEventArgs e )
        {
            var mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            this._screenMousePosition = new Vector2( mouseState.X, mouseState.Y );
            this._xnaMousePosition = this._camera.ToSpace( this._screenMousePosition );
            this.ScrollWheelValueDelta = this._previousMouseState.ScrollWheelValue - mouseState.ScrollWheelValue;

            var xnaMouseState = new XnaMouseState( this._xnaMousePosition, mouseState.LeftButton, this.ScrollWheelValueDelta );

            if ( mouseState.LeftButton != this._previousMouseState.LeftButton )
            {
                this.OnLeftButtonChanged( xnaMouseState );
            }

            var previousPositon = this._camera.ToSpace( new Vector2( this._previousMouseState.X, this._previousMouseState.Y ) );
            this.MouseMoveDelta = this.MouseXnaPosition - previousPositon;
            if ( this.MouseXnaPosition != previousPositon )
            {
                this._mousePositionChanged.OnNext( xnaMouseState );
            }

            if ( this.ScrollWheelValueDelta != 0 )
            {
                this._scrollWheelChanged.OnNext( xnaMouseState );
            }

            this._previousMouseState = mouseState;
        }

        private void OnLeftButtonChanged( XnaMouseState mouseState )
        {
            this._lefButtonChanged.OnNext( mouseState.LeftButton );
            if ( mouseState.LeftButton == ButtonState.Pressed )
            {
                this._lastMoseDownPosition = this._xnaMousePosition;
                this._leftButtonPressed.OnNext( mouseState );
            }
            else if ( mouseState.LeftButton == ButtonState.Released )
            {
                this._leftButtonReleased.OnNext( mouseState );
                if ( Vector2.DistanceSquared( this._lastMoseDownPosition, this._xnaMousePosition ) < ClickDistance )
                {
                    this._leftButtonClicked.OnNext( mouseState );
                }
            }
        }
    }
}
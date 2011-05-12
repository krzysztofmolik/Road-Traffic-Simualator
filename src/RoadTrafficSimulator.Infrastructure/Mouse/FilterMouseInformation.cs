using System;
using System.Collections.Generic;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RoadTrafficSimulator.Infrastructure.Mouse
{
    public class FilterMouseInformation : IMouseInformation
    {
        private readonly ISubject<XnaMouseState> _mousePositionChanged = new Subject<XnaMouseState>();
        private readonly ISubject<ButtonState> _leftButtonChanged = new Subject<ButtonState>();
        private readonly ISubject<XnaMouseState> _leftBtuttonRelease = new Subject<XnaMouseState>();
        private readonly ISubject<XnaMouseState> _leftButtonPressed = new Subject<XnaMouseState>();
        private readonly ISubject<XnaMouseState> _leftButtonClicked = new Subject<XnaMouseState>();
        private ISubject<XnaMouseState> _scrollWheelValueChanged = new Subject<XnaMouseState>();
        private readonly PriorityMouseInfomrmation _priorityMouseInformation;

        public FilterMouseInformation( PriorityMouseInfomrmation priorityMouseInformation )
        {
            this._priorityMouseInformation = priorityMouseInformation.NotNull();
        }

        public float XnaXMousePosition
        {
            get { return this._priorityMouseInformation.MouseInformation.XnaXMousePosition; }
        }

        public float XnaYMousePosition
        {
            get { return this._priorityMouseInformation.MouseInformation.XnaYMousePosition; }
        }

        public float ScreenXMousePosition
        {
            get { return this._priorityMouseInformation.MouseInformation.ScreenXMousePosition; }
        }

        public float ScreenYMousePosition
        {
            get { return this._priorityMouseInformation.MouseInformation.ScreenYMousePosition; }
        }

        public Vector2 MouseMoveDelta
        {
            get { return this._priorityMouseInformation.MouseInformation.MouseMoveDelta; }
        }

        public Vector2 MouseXnaPosition
        {
            get { return this._priorityMouseInformation.MouseInformation.MouseXnaPosition; }
        }

        public int ScrollWheelValueDelta
        {
            get { return this._priorityMouseInformation.MouseInformation.ScrollWheelValueDelta; }
        }

        public ISubject<XnaMouseState> MousePositionChangedSubject
        {
            get { return this._mousePositionChanged; }
        }

        public ISubject<ButtonState> LeftButtonChangedSubject
        {
            get { return this._leftButtonChanged; }
        }

        public ISubject<XnaMouseState> LeftButtonReleaseSubject
        {
            get { return this._leftBtuttonRelease; }
        }

        public ISubject<XnaMouseState> LeftButtonPressedSubject
        {
            get { return this._leftButtonPressed; }
        }

        public ISubject<XnaMouseState> LeftButtonClickedSubject
        {
            get { return this._leftButtonClicked; }
        }

        public ISubject<XnaMouseState> ScrollWheelValueDeltaSubject
        {
            get { return this._scrollWheelValueChanged; }
        }

        public IObservable<XnaMouseState> MousePositionChanged
        {
            get { return this._mousePositionChanged; }
        }

        public IObservable<ButtonState> LeftButtonChanged
        {
            get { return this._leftButtonChanged; }
        }

        public IObservable<XnaMouseState> LeftButtonRelease
        {
            get { return this._leftBtuttonRelease; }
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
            get { return this._scrollWheelValueChanged; }
        }

        public void StartRecord( int inteval )
        {
            this._priorityMouseInformation.Push( this );
        }

        public void StopRecord()
        {
            this._priorityMouseInformation.Pull( this );
        }

        public void Dispose()
        {
        }
    }
}
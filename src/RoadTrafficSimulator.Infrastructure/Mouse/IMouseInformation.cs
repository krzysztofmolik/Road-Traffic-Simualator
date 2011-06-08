using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RoadTrafficSimulator.Infrastructure.Mouse
{
    public interface IMouseInformation : IDisposable
    {
        IObservable<XnaMouseState> MousePositionChanged { get; }
        float XnaXMousePosition { get; }
        float XnaYMousePosition { get; }
        float ScreenXMousePosition { get; }
        float ScreenYMousePosition { get; }
        IObservable<ButtonState> LeftButtonChanged { get; }
        Vector2 MouseXnaPosition { get; }
        Vector2 MouseMoveDelta { get; }
        IObservable<XnaMouseState> LeftButtonRelease { get; }
        IObservable<XnaMouseState> LeftButtonPressed { get; }
        IObservable<XnaMouseState> LeftButtonClicked { get; }
        int ScrollWheelValueDelta { get; }
        IObservable<XnaMouseState> ScrollWheelChanged { get; }
        IObservable<XnaMouseState> DoubleClick { get; }
        void StartRecord( int inteval = 100 );
        void StopRecord();
    }
}
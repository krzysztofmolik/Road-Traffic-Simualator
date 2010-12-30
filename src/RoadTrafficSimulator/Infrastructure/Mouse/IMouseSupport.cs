using RoadTrafficSimulator.MouseHandler.Infrastructure;

namespace XnaRoadTrafficConstructor.Infrastucure.Mouse
{
    public interface IMouseSupport
    {
        void OnMove( XnaMouseState state );
        void OnLeftButtonClick( XnaMouseState state );
        void OnLeftButtonPressed( XnaMouseState state );
        void OnLeftButtonReleased( XnaMouseState state );
    }
}
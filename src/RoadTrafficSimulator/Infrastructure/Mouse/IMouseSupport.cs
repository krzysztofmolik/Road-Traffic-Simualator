using RoadTrafficSimulator.MouseHandler.Infrastructure;

namespace RoadTrafficSimulator.Infrastructure.Mouse
{
    public interface IMouseSupport
    {
        void OnMove( XnaMouseState state );

        void OnLeftButtonClick( XnaMouseState state );

        void OnLeftButtonPressed( XnaMouseState state );

        void OnLeftButtonReleased( XnaMouseState state );
    }
}
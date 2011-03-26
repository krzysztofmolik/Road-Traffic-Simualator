using RoadTrafficSimulator.MouseHandler.Infrastructure;

namespace RoadTrafficSimulator.Infrastructure.Mouse
{
    public interface IMouseHandler
    {
        void OnMove( XnaMouseState state );

        void OnLeftButtonClick( XnaMouseState state );

        void OnLeftButtonPressed( XnaMouseState state );

        void OnLeftButtonReleased( XnaMouseState state );
    }
}
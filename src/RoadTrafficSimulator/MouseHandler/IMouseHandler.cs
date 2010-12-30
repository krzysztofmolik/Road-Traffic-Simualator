using RoadTrafficSimulator.MouseHandler.Infrastructure;

namespace XnaRoadTrafficConstructor.MouseHandler
{
    public interface IMouseHandler
    {
        bool MouseDown(XnaMouseState mouseState );
        bool MouseClick( XnaMouseState mouseState );
        bool MouseMove( XnaMouseState mouseState );
        bool MouseUp( XnaMouseState mouseState );
    }
}
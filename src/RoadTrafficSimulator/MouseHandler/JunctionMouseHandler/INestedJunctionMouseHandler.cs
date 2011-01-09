using RoadTrafficSimulator.MouseHandler.Infrastructure;
using RoadTrafficSimulator.Road.Controls;

namespace RoadTrafficSimulator.MouseHandler.JunctionMouseHandler
{
    public interface INestedJunctionMouseHandler
    {
        bool MouseDown( XnaMouseState mouseState, IRoadJunctionBlock junction );
        bool MouseUp( XnaMouseState mouseState, IRoadJunctionBlock junction );
        bool MouseMove( XnaMouseState mouseState, IRoadJunctionBlock junction );
        bool MouseClick( XnaMouseState mouseState, IRoadJunctionBlock junction );
    }
}
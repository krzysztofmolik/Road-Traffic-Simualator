using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RoadTrafficSimulator.Infrastructure
{
    public class XnaMouseState
    {
        public XnaMouseState( Vector2 location, ButtonState leftButtonState, int scrollWheelValueDelta )
        {
            this.Location = location;
            this.LeftButton = leftButtonState;
            this.ScrollWheelValueDelta = scrollWheelValueDelta;
        }

        public Vector2 Location { get; private set; }
        public ButtonState LeftButton { get; private set; }
        public int ScrollWheelValueDelta { get; set; }

    }
}
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Xna
{
    public class CameraChangedEventArgs : EventArgs
    {
        public bool Handled { get; set; }
    }

    public class MouseStateEventArgs : EventArgs
    {
        public MouseStateEventArgs(Vector2 mousePosition, ButtonState leftButton, ButtonState rightButton)
        {
            MousePosition = mousePosition;
            LeftMouseState = leftButton;
            RightButton = rightButton;
        }

        public Vector2 MousePosition { get; private set; }

        public ButtonState LeftMouseState { get; private set; }

        public ButtonState RightButton { get; private set; }

        public bool Handled { get; set; }
    }
}
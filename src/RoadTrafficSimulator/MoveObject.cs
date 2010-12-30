using System;
using Microsoft.Xna.Framework.Input;

namespace Xna
{
    public class MoveObject
    {
        private int _lastX;
        private int _lastY;
        private bool _isLeftMousePressed;

        private Action<int, int> _updateLocation;

        public MoveObject(Action<int, int> updateLocation)
        {
            _updateLocation = updateLocation;
        }

        public bool StateChangedToPressed { get; set; }

        public bool StateChangedToRealease { get; set; }

        public bool IsMoved { get { return _isLeftMousePressed; } }

        public void Update(MouseState mouseState)
        {
            if(_isLeftMousePressed && mouseState.LeftButton == ButtonState.Pressed)
            {
                if(PositionChanged(_lastX, _lastY, mouseState.X, mouseState.Y))
                {
                    _updateLocation(_lastX, _lastY);
                }
            }

            _lastX = mouseState.X;
            _lastY = mouseState.Y;
            _isLeftMousePressed = ( mouseState.LeftButton == ButtonState.Pressed );
            
        }

        private static bool PositionChanged(int lastX, int lastY, int x, int y)
        {
            return (lastX != x) ||
                   (lastY != y);
        }
    }
}
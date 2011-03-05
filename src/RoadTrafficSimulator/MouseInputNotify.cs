using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RoadTrafficSimulator;
using XnaVs10;

namespace Xna
{
    public class MouseInputNotify
    {
        public event EventHandler<MouseStateEventArgs> MouseRelease;
        public event EventHandler<MouseStateEventArgs> MousePressed;
        public event EventHandler<MouseStateEventArgs> MouseMove;

        public Vector2 LastMousePosition { get; private set;}
        public ButtonState LeftMouseButtonState { get; private set; }

        //TODO To jest bez sensu
        public void Update(MouseState mouseState )
        {
            //var pointInControl = this._control.PointToClient( new System.Drawing.Point( mouseState.X, mouseState.Y ) );
            var mousePosition = new Vector2( mouseState.X, mouseState.Y );
            if( !this.IsUnderXnaControl(mousePosition))
            {
                return;
            }

            var mouseStateEventArgs = new MouseStateEventArgs(
                mousePosition, 
                mouseState.LeftButton,
                mouseState.RightButton);

            if(IsPressed(LeftMouseButtonState, mouseState.LeftButton))
            {
                MousePressed.Raise(this, mouseStateEventArgs);
            }

            if(IsRelease(LeftMouseButtonState, mouseState.LeftButton))
            {
                MouseRelease.Raise(this, mouseStateEventArgs);
            }

            if ( IsMoved( LastMousePosition, mousePosition ) )
            {
                MouseMove.Raise(this, mouseStateEventArgs);
            }

            LastMousePosition = mousePosition;
            LeftMouseButtonState = mouseState.LeftButton;

        }

        private static bool IsMoved(Vector2 position, Vector2 point)
        {
            return ( position.X != point.X ) || ( position.Y != point.Y ) ;
        }

        private static bool IsRelease(ButtonState previous, ButtonState actual)
        {
            return ((previous == ButtonState.Pressed) && (actual == ButtonState.Released));
        }

        private static bool IsPressed(ButtonState previous, ButtonState actual)
        {
            return ( (previous == ButtonState.Released) && (actual == ButtonState.Pressed) );
        }

        public bool IsUnderXnaControl( Vector2 mousePosition )
        {
            return mousePosition.X > 0 && mousePosition.Y > 0;
        }
    }
}
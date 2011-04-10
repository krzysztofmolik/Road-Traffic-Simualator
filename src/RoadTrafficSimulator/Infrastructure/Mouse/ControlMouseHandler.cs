using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.MouseHandler.Infrastructure;

namespace RoadTrafficSimulator.Infrastructure.Mouse
{
    public class SingleControlMouseHandler : IMouseHandler
    {
        private readonly IControl _owner;
        private readonly SelectedControls _selectedControls;
        private readonly MoveControl _moveControl;
        private Vector2 _clickOffset;

        public SingleControlMouseHandler( IControl owner, SelectedControls selectedControls, MoveControl moveControl )
        {
            this._owner = owner;
            this._selectedControls = selectedControls;
            this._moveControl = moveControl;
        }

        public void OnMove( XnaMouseState state )
        {
            var translationVector = state.Location - this._owner.Location + this._clickOffset;
            this._moveControl.Translate( this._owner, translationVector );
        }

        public void OnLeftButtonClick( XnaMouseState state )
        {
            this._selectedControls.ToggleSelection( this._owner );
        }

        public void OnLeftButtonPressed( XnaMouseState state )
        {
            this._clickOffset = this._owner.ToControlPosition( state.Location );
        }

        public void OnLeftButtonReleased( XnaMouseState state )
        {
            this._clickOffset = Vector2.Zero;
        }
    }
}
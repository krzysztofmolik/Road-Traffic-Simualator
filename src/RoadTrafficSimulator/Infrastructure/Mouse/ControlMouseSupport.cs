using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.MouseHandler.Infrastructure;
using XnaVs10.Extension;

namespace RoadTrafficSimulator.Infrastructure.Mouse
{
    public class ControlMouseSupport : IMouseSupport
    {
        private readonly IControl _owner;

        public ControlMouseSupport( IControl owner )
        {
            this._owner = owner;
        }

        public void OnMove( XnaMouseState state )
        {
            this.Move( state );
        }

        public void OnLeftButtonClick( XnaMouseState state )
        {
            this._owner.IsSelected = !this._owner.IsSelected;
        }

        public void OnLeftButtonPressed( XnaMouseState state )
        {
            this._owner.ToControlPosition( state.Location );
        }

        public void OnLeftButtonReleased( XnaMouseState state )
        {
        }

        private void Move( XnaMouseState state )
        {
            var moveVector = state.Location - this._owner.Location;
            this._owner.Translate( Matrix.CreateTranslation( moveVector.ToVector3() ) );
        }
    }
}
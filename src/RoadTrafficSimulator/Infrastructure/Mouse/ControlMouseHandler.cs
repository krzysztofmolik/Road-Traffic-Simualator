using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.MouseHandler.Infrastructure;
using XnaVs10.Extension;

namespace RoadTrafficSimulator.Infrastructure.Mouse
{
    public class SingleControlMouseHandler : IMouseHandler
    {
        private readonly IControl _owner;
        private readonly SelectedControls _selectedControls;

        public SingleControlMouseHandler( IControl owner, SelectedControls selectedControls )
        {
            this._owner = owner;
            this._selectedControls = selectedControls;
        }

        public void OnMove( XnaMouseState state )
        {
            this.Move( state );
        }

        public void OnLeftButtonClick( XnaMouseState state )
        {
            var seleected = !this._owner.IsSelected;
            if ( seleected )
            {
                this._selectedControls.Add( this._owner );
            }
            else
            {
                this._selectedControls.Remove( this._owner );
            }
            this._owner.IsSelected = seleected;
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
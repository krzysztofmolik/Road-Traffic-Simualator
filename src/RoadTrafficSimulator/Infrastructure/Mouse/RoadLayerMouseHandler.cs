using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.MouseHandler.Infrastructure;
using RoadTrafficSimulator.Road;

namespace RoadTrafficSimulator.Infrastructure.Mouse
{
    // TODO Movie it to more appropiate place
    public class RoadLayerMouseHandler : IMouseHandler
    {
        private readonly RoadLayer _owner;
        private readonly SelectedControls _selectedControls;

        private IControl _mouseOwner;

        public RoadLayerMouseHandler( RoadLayer owner, SelectedControls selectedControls )
        {
            this._owner = owner;
            this._selectedControls = selectedControls;
        }

        public void OnMove( XnaMouseState state )
        {
            if ( this._mouseOwner == null )
            {
                return;
            }

            if ( this._mouseOwner.IsSelected )
            {
                this._selectedControls.ForEach( c => c.MouseSupport.OnMove( state ) );
            }
            else
            {
                this._mouseOwner.MouseSupport.OnMove( state );
            }

        }

        public void OnLeftButtonClick( XnaMouseState state )
        {
            var control = this.FindControlAtPoint( state.Location ) as IControl;
            if ( control != null )
            {
                control.MouseSupport.OnLeftButtonClick( state );
            }
            else
            {
                this._selectedControls.Clear();
            }
        }

        public void OnLeftButtonPressed( XnaMouseState state )
        {
            Debug.Assert( this._mouseOwner == null, "this._selectedControl == null" );
            //TODO CHange it
            this._mouseOwner = this.FindControlAtPoint( state.Location ) as IControl;
            if ( this._mouseOwner != null )
            {
                this._mouseOwner.MouseSupport.OnLeftButtonPressed( state );
            }
            else
            {
                this._selectedControls.Clear();
            }
        }

        private ILogicControl FindControlAtPoint( Vector2 location )
        {
            return this._owner.Children.FirstOrDefault( s => s.IsHitted( location ) );
        }

        public void OnLeftButtonReleased( XnaMouseState state )
        {
            if ( this._mouseOwner == null )
            {
                return;
            }

            this._mouseOwner.MouseSupport.OnLeftButtonReleased( state );
            this._mouseOwner = null;
        }
    }
}
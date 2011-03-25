using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.MouseHandler.Infrastructure;
using RoadTrafficSimulator.Road;

namespace RoadTrafficSimulator.Infrastructure.Mouse
{
    // TODO Movie it to more appropiate place
    public class RoadLayerMouseSupport : IMouseSupport
    {
        private readonly RoadLayer _owner;

        private IControl _selectedControlBase;

        public RoadLayerMouseSupport( RoadLayer owner )
        {
            this._owner = owner;
        }

        public void OnMove( XnaMouseState state )
        {
            if ( this._selectedControlBase == null )
            {
                return;
            }

            this._selectedControlBase.MouseSupport.OnMove( state );
        }

        public void OnLeftButtonClick( XnaMouseState state )
        {
            //TODO Change it
            var control = this.FindControlAtPoint( state.Location ) as IControl;
            if ( control != null )
            {
                control.MouseSupport.OnLeftButtonClick( state );
            }
        }

        public void OnLeftButtonPressed( XnaMouseState state )
        {
            Debug.Assert( this._selectedControlBase == null, "this._selectedControl == null" );
            //TODO CHange it
            this._selectedControlBase = this.FindControlAtPoint( state.Location ) as IControl;
            if ( this._selectedControlBase != null )
            {
                this._selectedControlBase.MouseSupport.OnLeftButtonPressed( state );
            }
        }

        private ILogicControl FindControlAtPoint( Vector2 location )
        {
            var hitedControl = this._owner.Children.FirstOrDefault( s => s.IsHitted( location ) );
            if( hitedControl != null )
            {
                return hitedControl.GetHittedControl(location);
            }
            return null;
        }

        public void OnLeftButtonReleased( XnaMouseState state )
        {
            if ( this._selectedControlBase == null )
            {
                return;
            }

            this._selectedControlBase.MouseSupport.OnLeftButtonReleased( state );
            this._selectedControlBase = null;
        }
    }
}
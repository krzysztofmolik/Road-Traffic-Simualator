using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Infrastructure.Mouse
{
    public class CompositeControlMouseHandler : IMouseHandler
    {
        private readonly ICompositeControl _owner;
        private readonly SelectedControls _selectedControls;
        private readonly MoveControl _moveControl;

        private IControl _selectedControlBase;
        private Vector2 _selctedControlOffset;

        public CompositeControlMouseHandler( ICompositeControl owner, SelectedControls selectedControls, MoveControl moveControl )
        {
            this._owner = owner;
            this._moveControl = moveControl;
            this._selectedControls = selectedControls;
        }

        public void OnMove( XnaMouseState state )
        {
            if ( this._selectedControlBase == null )
            {
                return;
            }

            this.Move( state );
        }

        private void Move( XnaMouseState state )
        {
            // TODO WTF
            if ( this._selectedControlBase != this._owner && this._selectedControlBase.MouseHandler != null )
            {
                this._selectedControlBase.MouseHandler.OnMove(state);
            }
            else
            {
                var moveVector = state.Location - this._owner.Location + this._selctedControlOffset;
                this._moveControl.Translate( this._owner, moveVector );
            }
        }

        public void OnLeftButtonClick( XnaMouseState state )
        {
            // TODO CHange it
            var control = this.FindControlAtPoint( state.Location ) as IControl;
            if ( control != null && control != this._owner )
            {
                control.MouseHandler.OnLeftButtonClick( state );
            }
            else
            {
                this._selectedControls.ToggleSelection( this._owner );
            }
        }

        public void OnLeftButtonPressed( XnaMouseState state )
        {
            Debug.Assert( this._selectedControlBase == null, "this._selectedControl == null" );
            //TODO Change it
            this._selectedControlBase = this.FindControlAtPoint( state.Location ) as IControl;
            if ( this._selectedControlBase == null )
            {
                this._selectedControlBase = this._owner;
                Debug.Assert( this._owner.IsHitted( state.Location ), "this._owner.HitTest(state.Location)" );
            }

            if ( this._selectedControlBase != null )
            {
                this._selctedControlOffset = this._selectedControlBase.ToControlPosition( state.Location );
                this.LeftButtonPressed( state );
            }

        }

        private void LeftButtonPressed( XnaMouseState state )
        {
            // TODO O co chodzi
            if ( this._selectedControlBase != this._owner && this._selectedControlBase.MouseHandler != null )
            {
                this._selectedControlBase.MouseHandler.OnLeftButtonPressed( state );
            }
        }

        private ILogicControl FindControlAtPoint( Vector2 location )
        {
            var hittedControl = this._owner.Children.FirstOrDefault( s => s.IsHitted( location ) );
            if ( hittedControl != null )
            {
                return hittedControl.GetHittedControl( location ) ?? this._owner;
            }

            return this._owner;
        }

        public void OnLeftButtonReleased( XnaMouseState state )
        {
            if ( this._selectedControlBase == null )
            {
                return;
            }

            this.LeftButtonReleased( state );

            this._selectedControlBase = null;
        }

        private void LeftButtonReleased( XnaMouseState state )
        {
            if ( this._selectedControlBase == null )
            {
                return;
            }
            if ( this._selectedControlBase != this._owner )
            {
                this._selectedControlBase.MouseHandler.OnLeftButtonReleased( state );
            }
            else
            {

            }
        }
    }
}
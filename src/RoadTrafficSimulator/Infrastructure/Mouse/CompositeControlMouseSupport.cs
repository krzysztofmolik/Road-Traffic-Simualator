using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Common;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Factories;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.MouseHandler.Infrastructure;
using RoadTrafficSimulator.Road;
using RoadTrafficSimulator.Road.Controls;
using XnaVs10.Extension;
using RoadTrafficSimulator.Extension;

namespace RoadTrafficSimulator.Infrastructure.Mouse
{
    public class CompositeControlMouseSupport : IMouseSupport
    {
        private readonly ICompositeControl _owner;

        private IControl _selectedControlBase;
        private Vector2 _selctedControlOffset;

        public CompositeControlMouseSupport( ICompositeControl owner )
        {
            this._owner = owner;
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
            if ( this._selectedControlBase != this._owner )
            {
                this._selectedControlBase.MouseSupport.OnMove( state );
            }
            else
            {
                var moveVector = state.Location - this._owner.Location + this._selctedControlOffset;
                this._owner.Translate( Matrix.CreateTranslation( moveVector.ToVector3() ) );
            }
        }

        public void OnLeftButtonClick( XnaMouseState state )
        {
            // TODO CHange it
            var control = this.FindControlAtPoint( state.Location ) as IControl;
            if ( control != null && control != this._owner )
            {
                control.MouseSupport.OnLeftButtonClick( state );
            }
            else
            {
                this._owner.IsSelected = !this._owner.IsSelected;
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
            if ( this._selectedControlBase != this._owner )
            {
                this._selectedControlBase.MouseSupport.OnLeftButtonPressed( state );
            }
        }

        private ILogicControl FindControlAtPoint( Vector2 location )
        {
            var hitChildren = this._owner.Children.FirstOrDefault( s => s.IsHitted( location ) );
            if ( hitChildren != null )
            {
                return hitChildren.GetHittedControl( location ) ?? this._owner;
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
                this._selectedControlBase.MouseSupport.OnLeftButtonReleased( state );
            }
            else
            {

            }
        }
    }
}
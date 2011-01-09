using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Common;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.MouseHandler.Infrastructure;
using RoadTrafficSimulator.Road;
using XnaVs10.Extension;

namespace RoadTrafficSimulator.Infrastructure.Mouse
{
    public interface IConnectionCommand
    {
        bool Connect( IControl first, IControl second );
    }

    public interface ISelectionSupport
    {
        IEnumerable<IControl> GetSelectedControls();
    }

    public class DefaultControlSelectionSupport : ISelectionSupport
    {
        private readonly IControl _owner;

        public DefaultControlSelectionSupport( IControl owner )
        {
            this._owner = owner.NotNull();
        }

        public IEnumerable<IControl> GetSelectedControls()
        {
            return this._owner.IsSelected ? new[] { this._owner } : Enumerable.Empty<IControl>();
        }
    }

    public class DefaultCompositeControlSelectionSupport : ISelectionSupport
    {
        private readonly ICompositeControl _owner;

        public DefaultCompositeControlSelectionSupport( ICompositeControl owner )
        {
            this._owner = owner.NotNull();
            this._owner.IsSelectedChanged.Subscribe( this.SelectionChanged );
        }

        private void SelectionChanged( bool isSelected )
        {
            if ( isSelected == false )
            {
                return;
            }

            this.GetSelectedControlsInternal().Where( s => s != this._owner ).ForEach( s => s.IsSelected = false );
        }

        private IEnumerable<IControl> GetSelectedControlsInternal()
        {
            var root = this._owner.GetRoot();
            var rootComposite = root as ICompositeControl;
            if ( rootComposite == null )
            {
                return root.IsSelected ? new[] { root } : Enumerable.Empty<IControl>();
            }
            var selected = rootComposite.Children.SelectMany( c => c.SelectionSupport.GetSelectedControls() );
            return selected;
        }

        public IEnumerable<IControl> GetSelectedControls()
        {
            var selectedControl = this._owner.Children.SelectMany( s => s.SelectionSupport.GetSelectedControls() );
            if ( this._owner.IsSelected )
            {
                return new[] { this._owner }.Concat( selectedControl );
            }

            return selectedControl;
        }
    }

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
            var control = this.FindControlAtPoint( state.Location );
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
            this._selectedControlBase = this.FindControlAtPoint( state.Location );
            if ( this._selectedControlBase == null )
            {
                this._selectedControlBase = this._owner;
                Debug.Assert( this._owner.HitTest( state.Location ), "this._owner.HitTest(state.Location)" );
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

        private IControl FindControlAtPoint( Vector2 location )
        {
            var hitChildren = this._owner.Children.FirstOrDefault( s => s.HitTest( location ) );
            return hitChildren ?? this._owner;
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
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using RoadTrafficSimulator.Infrastructure.Control;

namespace RoadTrafficSimulator.Infrastructure.Mouse
{
    public class SelectedControls
    {
        private readonly object _lock = new object();
        private readonly List<IControl> _selectedControls = new List<IControl>();
        private readonly KeyboardInputNotify _keyboard;

        public SelectedControls( KeyboardInputNotify keyboard )
        {
            this._keyboard = keyboard;
        }

        public void ToggleSelection( IControl control )
        {
            if ( control.IsSelected )
            {
                this.Unselect( control );
            }
            else
            {
                this.Select( control );
            }
        }

        public void Select( IControl control )
        {
            lock ( this._lock )
            {
                if ( IsMultiSelect() )
                {
                    this._selectedControls.ForEach( c => c.IsSelected = false );
                }

                control.IsSelected = true;
                this._selectedControls.Add( control );
            }
        }

        public void Unselect( IControl control )
        {
            lock ( this._lock )
            {
                control.IsSelected = false;
                this._selectedControls.Remove( control );
            }
        }

        public void Clear()
        {
            if( this.IsMultiSelect() )
            {
                return;
            }

            lock ( this._lock )
            {
                this._selectedControls.ForEach( s => s.IsSelected = false );
                this._selectedControls.Clear();
            }
        }

        public bool Contains(IControl control)
        {
            lock (_lock)
            {
                return this._selectedControls.Contains(control);
            }
        }

        internal void ForEach( Action<IControl> action )
        {
            lock ( this._lock )
            {
                this._selectedControls.ForEach( action );
            }
        }

        private bool IsMultiSelect()
        {
            return this._keyboard.IsKeyPressed( Keys.LeftControl ) == false && this._keyboard.IsKeyPressed( Keys.RightControl ) == false;
        }
    }
}
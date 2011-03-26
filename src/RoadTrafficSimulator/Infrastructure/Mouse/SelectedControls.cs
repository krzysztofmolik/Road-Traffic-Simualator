using System;
using System.Collections.Generic;
using RoadTrafficSimulator.Infrastructure.Control;

namespace RoadTrafficSimulator.Infrastructure.Mouse
{
    public class SelectedControls
    {
        private readonly object _lock = new object();
        private readonly List<IControl> _selectedControls = new List<IControl>();

        public void Add( IControl control )
        {
            lock ( this._lock )
            {
                this._selectedControls.Add( control );
            }
        }

        public void Remove( IControl control )
        {
            lock ( this._lock )
            {
                this._selectedControls.Remove( control );
            }
        }

        public void Clear()
        {
            lock ( this._lock )
            {
                this._selectedControls.Clear();
            }
        }

        public void ForEach( Action<IControl> action )
        {
            lock ( this._lock )
            {
                this._selectedControls.ForEach( action );
            }
        }
    }
}
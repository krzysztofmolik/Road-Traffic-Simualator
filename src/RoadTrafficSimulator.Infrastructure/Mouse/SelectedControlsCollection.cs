using System;
using System.Collections.Generic;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Infrastructure.Mouse
{
    public class SelectedControlsCollection
    {
        private readonly object _lock = new object();
        // TODO Change to new concurent collection
        private readonly List<IControl> _selectedControl = new List<IControl>();

        public void ForEach( Action<IControl> action )
        {
            lock ( this._lock )
            {
                this._selectedControl.ForEach( action );
            }
        }

        public void ReplaceWith( IControl control )
        {
            lock ( this._lock )
            {
                this._selectedControl.Clear();
                this._selectedControl.Add( control );
            }
        }

        public void Add( IControl control )
        {
            lock ( this._lock )
            {
                this._selectedControl.Add( control );
            }
        }

        public void ClearSelection()
        {
            lock (this._lock)
            {
                this._selectedControl.Clear();
            }
        }
    }
}
using System;
using System.Collections.Generic;

namespace RoadTrafficSimulator.Components.SimulationMode.Route
{
    public class Route : IRouteMark
    {
        private readonly List<IRoadElement> _route = new List<IRoadElement>();
        private RouteMark _routeMark;
        public void Add( IRoadElement roadElement )
        {
            this._route.Add( roadElement );
            this._routeMark = new RouteMark( this );
        }

        public IRoadElement Current
        {
            get { return this._routeMark.Current; }
        }

        public bool IsLast { get { return this._routeMark.IsLast; } }

        public bool IsValidIndex( int index )
        {
            return index >= 0 && index < this._route.Count;
        }

        public IRoadElement GetAt( int index )
        {
            if ( this.IsValidIndex( index ) == false ) { throw new ArgumentException( "Index is not valid", "index" ); }
            return this._route[ index ];
        }

        public int GetIndexOf( IRoadElement roadElement )
        {
            return this._route.FindIndex( s => s == roadElement );
        }

        public void SetLoctionOn( IRoadElement roadElement )
        {
            this._routeMark.SetLoctionOn( roadElement );
        }

        public IRoadElement GetPrevious()
        {
            return this._routeMark.GetPrevious();
        }

        public IRoadElement GetNext()
        {
            return this._routeMark.GetNext();
        }

        public bool MoveNext()
        {
            return this._routeMark.MoveNext();
        }

        public IRouteMark MovePrevious()
        {
            this._routeMark.MovePrevious();
            return this._routeMark;
        }

        public IRouteMark Clone()
        {
            return this._routeMark.Clone();
        }
    }
}
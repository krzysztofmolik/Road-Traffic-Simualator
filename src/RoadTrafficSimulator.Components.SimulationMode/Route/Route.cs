using System;
using System.Collections.Generic;

namespace RoadTrafficSimulator.Components.SimulationMode.Route
{
    public class Route<T> : IRouteMark<T>
    {
        private readonly List<T> _route = new List<T>();
        private RouteMark<T> _routeMark;

        public void Add( T roadElement )
        {
            this._route.Add( roadElement );
            this._routeMark = new RouteMark<T>( this );
        }

        public T Current
        {
            get { return this._routeMark.Current; }
        }

        public bool IsLast { get { return this._routeMark.IsLast; } }

        public bool IsValidIndex( int index )
        {
            return index >= 0 && index < this._route.Count;
        }

        public T GetAt( int index )
        {
            if ( this.IsValidIndex( index ) == false ) { throw new ArgumentException( "Index is not valid", "index" ); }
            return this._route[ index ];
        }

        public int GetIndexOf( T roadElement )
        {
            return this._route.FindIndex( s => Equals( s, roadElement ) );
        }

        public void SetLoctionOn( T roadElement )
        {
            this._routeMark.SetLoctionOn( roadElement );
        }

        public T GetPrevious()
        {
            return this._routeMark.GetPrevious();
        }

        public T GetNext()
        {
            return this._routeMark.GetNext();
        }

        public bool MoveNext()
        {
            return this._routeMark.MoveNext();
        }

        public IRouteMark<T> Clone()
        {
            return this._routeMark.Clone();
        }
    }
}
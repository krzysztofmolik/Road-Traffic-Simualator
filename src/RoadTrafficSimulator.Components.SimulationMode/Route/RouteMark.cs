using System;

namespace RoadTrafficSimulator.Components.SimulationMode.Route
{
    public class RouteMark<T> : IRouteMark<T>
    {
        private readonly Route<T> _route;
        private int _mark;

        public RouteMark( Route<T> route )
            : this( route, -1 ) { }

        public RouteMark( Route<T> route, int mark )
        {
            this._route = route;
            this._mark = mark;
        }

        public T Current { get { return this._route.GetAt( this._mark ); } }

        public bool IsLast
        {
            get { return !this._route.IsValidIndex( this._mark + 1 ); }
        }

        public void SetLoctionOn( T roadElement )
        {
            var index = this._route.GetIndexOf( roadElement );
            if ( index < 0 ) { throw new ArgumentException( "Element is not present in route", "roadElement" ); }
            this._mark = index;
        }

        public T GetPrevious()
        {
            var mark = this._mark - 1;
            if ( this._route.IsValidIndex( mark ) == false ) { return default( T ); }
            return this._route.GetAt( mark );
        }

        public T GetNext()
        {
            var mark = this._mark + 1;
            if ( this._route.IsValidIndex( mark ) == false ) { return default( T ); }
            return this._route.GetAt( mark );
        }

        public bool MoveNext()
        {
            if ( this._route.IsValidIndex( this._mark + 1 ) == false ) { return false; }
            ++this._mark;
            return true;
        }

        public bool MoveBack()
        {
            if ( this._route.IsValidIndex( this._mark - 1 ) == false ) { return false; }
            --this._mark;
            return true;
        }

        public IRouteMark<T> Clone()
        {
            return new RouteMark<T>( this._route, this._mark );
        }
    }
}
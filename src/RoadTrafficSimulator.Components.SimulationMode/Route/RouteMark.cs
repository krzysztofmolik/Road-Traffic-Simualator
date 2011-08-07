using System;

namespace RoadTrafficSimulator.Components.SimulationMode.Route
{
    public class RouteMark : IRouteMark
    {
        private readonly Route _route;
        private int _mark;

        public RouteMark( Route route )
            : this( route, 0 ) { }

        public RouteMark( Route route, int mark )
        {
            this._route = route;
            this._mark = mark;
        }

        public IRoadElement Current { get { return this._route.GetAt( this._mark ); } }

        public bool IsLast
        {
            get { return !this._route.IsValidIndex( this._mark + 1 ); }
        }

        public void SetLoctionOn( IRoadElement roadElement )
        {
            var index = this._route.GetIndexOf( roadElement );
            if ( index < 0 ) { throw new ArgumentException( "Element is not present in route", "roadElement" ); }
            this._mark = index;
        }

        public IRoadElement GetPrevious()
        {
            var mark = this._mark - 1;
            if ( this._route.IsValidIndex( mark ) == false ) { return null; }
            return this._route.GetAt( mark );
        }

        public IRoadElement GetNext()
        {
            var mark = this._mark + 1;
            if ( this._route.IsValidIndex( mark ) == false ) { return null; }
            return this._route.GetAt( mark );
        }

        public bool MoveNext()
        {
            ++this._mark;
            if ( this._route.IsValidIndex( this._mark ) == false ) { return false; }
            return true;
        }

        public IRouteMark MovePrevious()
        {
            --this._mark;
            if ( this._route.IsValidIndex( this._mark ) == false ) { throw new InvalidOperationException( "Can't move mark outside the route" ); }
            return this;
        }

        public IRouteMark Clone()
        {
            return new RouteMark( this._route, this._mark );
        }
    }
}
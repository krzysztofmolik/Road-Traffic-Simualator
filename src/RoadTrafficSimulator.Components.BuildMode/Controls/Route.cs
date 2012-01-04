using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class RouteElement
    {
        public RouteElement( IControl control, PriorityType priorityType )
        {
            this.Control = control;
            this.PriorityType = priorityType;
            this.CanStop = true;
        }

        public IControl Control { get; set; }
        public PriorityType PriorityType { get; set; }
        public bool CanStop { get; set; }
    }

    public class Route
    {
        private readonly List<RouteElement> _route;
        //        public Route()
        //            : this( Enumerable.Empty<RouteElement>(), 0 )
        //        {
        //            this._route = new List<RouteElement>();
        //        }

        public Route( string name, int probability, IRouteOwner owner )
        {
            this.Name = name;
            this.Probability = probability;
            this._route = new List<RouteElement>();
            this.Owner = owner;
        }

        public IRouteOwner Owner { get; set; }

        public Route( IEnumerable<RouteElement> routeElements, int probability, IRouteOwner owner )
        {
            this._route = new List<RouteElement>( routeElements );
            this.Probability = probability;
            this.Name = "Unknown";
            this.Owner = owner;
        }

        public string Name { get; set; }
        public int Probability { get; set; }
        public IEnumerable<RouteElement> Items { get { return this._route; } }

        public bool CanAdd( IControl control )
        {
            if ( this._route.IsEmpty() ) { return true; }

            // NOTE This only fake
            return true;
        }

        public void Add( IControl control, PriorityType priorityType, bool canStop )
        {
            this._route.Add( new RouteElement( control, priorityType ) { CanStop = canStop } );
        }

        public IEnumerable<PriorityType> GetPrioritiesFor( IControl control )
        {
            return new[]
                       {
                           PriorityType.Light,
                           PriorityType.FromLeft,
                           PriorityType.FromFront,
                           PriorityType.FromRight,
                       };
        }

        public IEnumerable<RouteElement> Add( IRouteElement routeElement )
        {
            var last = this._route.LastOrDefault();
            if ( last == null )
            {
                var pathFromOwner = this.GetPath( ( IRouteElement ) this.Owner, routeElement ).Select( r => new RouteElement( r, PriorityType.None ) ).ToArray();
                this._route.AddRange( pathFromOwner );
                return pathFromOwner;
            }

            var destinationPoint = last.Control as IRouteElement;
            if ( destinationPoint == null ) { throw new ArgumentException(); }

            var path = this.GetPath( ( IRouteElement ) last.Control, routeElement ).Select( r => new RouteElement( r, PriorityType.None ) ).ToArray();

            this._route.AddRange( path );
            return path;
        }

        private IEnumerable<IRouteElement> GetPath( IRouteElement from, IRouteElement to )
        {
            return new PathFinder().GetPath( from, to );
        }

        public void Remove( IControl control )
        {
            this._route.RemoveAll( r => r.Control == control );
        }
    }

    public class PathFinder
    {
        public IEnumerable<IRouteElement> GetPath( IRouteElement from, IRouteElement to )
        {

            var visitedControls = new List<IRouteElement>();

            var found = this.Check( from, visitedControls, new List<IRouteElement>(), to );
            return found.Path;
        }

        private SearchResult Check( IRouteElement routeElement, List<IRouteElement> vistedControls, List<IRouteElement> currentPath, IRouteElement destination )
        {
            if ( routeElement == destination )
            {
                return new SearchResult( true, currentPath );
            }

            if ( currentPath.Count >= 3 )
            {
                return new SearchResult( false, Enumerable.Empty<IRouteElement>() );
            }


            if ( vistedControls.Contains( routeElement ) ) { return new SearchResult( false, Enumerable.Empty<IRouteElement>() ); }
            vistedControls.Add( routeElement );


            foreach ( var connectedControl in routeElement.GetConnectedControls() )
            {
                var newPaht = new List<IRouteElement>( currentPath ) { connectedControl };
                var found = this.Check( connectedControl, vistedControls, newPaht, destination );
                if ( found.Found ) { return found; }
            }

            return new SearchResult( false, Enumerable.Empty<IRouteElement>() );
        }

        private class SearchResult
        {
            public SearchResult( bool found, IEnumerable<IRouteElement> path )
            {
                this.Found = found;
                this.Path = path;
            }

            public bool Found { get; private set; }
            public IEnumerable<IRouteElement> Path { get; private set; }

        }
    }

}
using System;
using System.Collections.Generic;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class Routes
    {
        private readonly List<Route> _routes;

        public Routes()
        {
            this._routes = new List<Route>();
        }

        public IEnumerable<Route> AvailableRoutes { get { return this._routes; } }

        public void AddRoute( params RouteElement[] controls )
        {
            var route = new Route( controls, 100 );
            this._routes.Add( route );
        }

        public void AddRoute( Route route )
        {
            if (route == null) throw new ArgumentNullException("route");
            this._routes.Add( route );
        }

        public void Remove( Route route )
        {
            if( route == null ) throw new ArgumentNullException( "route" );
            this._routes.Remove( route );
        }
    }
}
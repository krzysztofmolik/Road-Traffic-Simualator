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
            var route = new Route( controls, 100.0f );
            this._routes.Add( route );
        }

    }
}
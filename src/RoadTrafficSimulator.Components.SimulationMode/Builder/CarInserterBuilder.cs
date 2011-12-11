using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public class CarInserterBuilder : IBuilerItem
    {
        public IEnumerable<BuilderAction> Create( IControl control )
        {
            var builder = new Builder();
            yield return new BuilderAction( Order.High, context => builder.Build( context, control ) );
            yield return new BuilderAction( Order.Normal, builder.Connect );
            yield return new BuilderAction( Order.Low, builder.SetUp );
        }

        public bool CanCreate( IControl control )
        {
            return control != null && control.GetType() == typeof( BuildMode.Controls.CarsInserter );
        }

        private class Builder
        {
            private CarsInserter _carsInserter;
            private readonly BuildRoutesToSimulationRoutesConverter _converter = new BuildRoutesToSimulationRoutesConverter();

            public void Build( BuilderContext context, IControl control )
            {
                var orignalCarInserter = ( BuildMode.Controls.CarsInserter ) control;
                this._carsInserter = new CarsInserter( orignalCarInserter, c => context.RoadInformationFactory.Create( c ) );
                context.AddElement( orignalCarInserter, this._carsInserter );
            }

            public void Connect( BuilderContext builderContext )
            {
                var connectedLane = this._carsInserter.CarsInserterBuilder.Connector.OpositeEdge;
                if ( connectedLane == null ) { return; }
                this._carsInserter.Lane = builderContext.GetObject<Lane>( connectedLane.Parent );
            }

            public void SetUp( BuilderContext obj )
            {
                var routes = this._carsInserter.CarsInserterBuilder.Routes;
                this._carsInserter.Routes = new Routes( this._converter.Convert( routes.AvailableRoutes, obj ).ToArray() );
            }
        }
    }

    public class BuildRoutesToSimulationRoutesConverter
    {
        public IEnumerable<BuildRoute> Convert( IEnumerable<BuildMode.Controls.Route> buildRoutes, BuilderContext context )
        {
            return buildRoutes.Select( route => this.ConvertRoute( route, context ) );
        }

        private BuildRoute ConvertRoute( BuildMode.Controls.Route route, BuilderContext context )
        {
            return new BuildRoute( this.GetRouteElements( route, context ) )
                                      {
                                          Probability = route.Probability,
                                          Name = route.Name,
                                      };
        }

        private IEnumerable<RouteElement> GetRouteElements( BuildMode.Controls.Route route, BuilderContext context )
        {
            return route.Items.Select( r => new RouteElement { PriorityType = r.PriorityType, RoadElement = context.GetObject<IRoadElement>( r.Control ) } );
        }
    }

    public class Routes
    {
        private readonly static Routes _empty = new Routes( Enumerable.Empty<BuildRoute>() );

        public static Routes Empty
        {
            get { return _empty; }
        }

        private readonly List<BuildRoute> _routes;

        public Routes( IEnumerable<BuildRoute> routes )
        {
            this._routes = new List<BuildRoute>( routes );
        }

        public IEnumerable<RouteElement> GetRandomRoute( Random rng )
        {
            var rngNumber = rng.Next( 0, 100 );
            var previous = 0.0;
            for ( var i = 0; i < this._routes.Count; i++ )
            {
                if ( previous < rngNumber && rngNumber <= this._routes[ i ].Probability )
                {
                    return this._routes[ i ].Elements;
                }
                previous = this._routes[ i ].Probability;
            }

            return Enumerable.Empty<RouteElement>();
        }

        public void CalculateProbabilities()
        {
            var alreadySet = this._routes.Where( s => s.Probability != 0 ).ToArray();
            var inUse = alreadySet.Sum( s => s.Probability );

            var left = this._routes.Count - alreadySet.Length;
            if ( left == 0 ) { return; }

            var perItem = ( 100 - inUse ) / left;
            this._routes.Where( s => s.Probability == 0 ).ForEach( s => s.Probability = perItem );
        }

        public void Add( BuildRoute route )
        {
            this._routes.Add( route );
        }
    }

    public class RouteElement
    {
        public IRoadElement RoadElement { get; set; }
        public PriorityType PriorityType { get; set; }
        public bool CanStopOnIt { get; set; }
    }

    public class BuildRoute
    {
        private readonly List<RouteElement> _elements;

        public BuildRoute( IEnumerable<RouteElement> elements )
        {
            this._elements = new List<RouteElement>( elements );
        }

        public IEnumerable<RouteElement> Elements { get { return this._elements; } }

        public float Probability { get; set; }

        public string Name { get; set; }
    }
}
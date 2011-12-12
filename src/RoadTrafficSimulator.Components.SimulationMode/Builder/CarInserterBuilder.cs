using System.Collections.Generic;
using System.Linq;
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
                this._carsInserter.Routes = new StandardRoutes( this._converter.Convert( routes.AvailableRoutes, obj ).ToArray() );
            }
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
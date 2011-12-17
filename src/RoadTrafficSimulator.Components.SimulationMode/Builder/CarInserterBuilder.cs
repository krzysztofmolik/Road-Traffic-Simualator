using System.Collections.Generic;
using System.Linq;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;
using CarsInserter = RoadTrafficSimulator.Components.SimulationMode.Elements.CarsInserter;

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
            yield return new BuilderAction( Order.VeryLow, builder.SetConnection );
        }

        public bool CanCreate( IControl control )
        {
            return control != null && control.GetType() == typeof( BuildMode.Controls.CarsInserter );
        }

        private class Builder : BuilderBase
        {
            private CarsInserter _carsInserter;

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
                var convertedRoutes = this.ConvertRoutes( routes, obj, this._carsInserter ).ToArray();
                this._carsInserter.Routes = new StandardRoutes( convertedRoutes );
            }

            public void SetConnection( BuilderContext context )
            {
                this.SetConnections( this._carsInserter.Routes.AvailableRoutes );
            }
        }
    }
}
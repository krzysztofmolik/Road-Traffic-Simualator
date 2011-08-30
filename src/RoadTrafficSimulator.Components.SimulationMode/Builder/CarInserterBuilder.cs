using System.Collections.Generic;
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
        }

        public bool CanCreate( IControl control )
        {
            return control != null && control.GetType() == typeof( BuildMode.Controls.CarsInserter );
        }

        private class Builder
        {
            private CarsInserter _carsInserter;

            public void Build( BuilderContext context, IControl control )
            {
                var roadJunctionBlock = ( BuildMode.Controls.CarsInserter ) control;
                this._carsInserter = new CarsInserter( roadJunctionBlock, c => context.RoadInformationFactory.Create( c ) );
                context.AddElement( roadJunctionBlock, this._carsInserter );
            }

            public void Connect( BuilderContext builderContext )
            {
                var connectedLane = this._carsInserter.CarsInserterBuilder.Connector.OpositeEdge;
                if ( connectedLane == null ) { return; }
                this._carsInserter.Lane = builderContext.GetObject<Lane>( connectedLane.Parent );
            }
        }
    }
}
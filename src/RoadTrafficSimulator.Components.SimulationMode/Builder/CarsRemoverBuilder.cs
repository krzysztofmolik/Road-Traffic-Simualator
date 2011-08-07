using System.Collections.Generic;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;
using CarsRemoverBuildMode = RoadTrafficSimulator.Components.BuildMode.Controls.CarsRemover;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public class CarsRemoverBuilder : IBuilerItem
    {
        public IEnumerable<BuilderAction> Create( IControl control )
        {
            var builder = new Builder();
            yield return new BuilderAction( Order.High, context => builder.Build( context, control ) );
            yield return new BuilderAction( Order.Normal, builder.Connect );
        }

        public bool CanCreate( IControl control )
        {
            return control != null && control.GetType() == typeof( CarsRemoverBuildMode );
        }

        public class Builder
        {
            private CarsRemover _carsRemover;

            public void Build( BuilderContext context, IControl control )
            {
                var roadJunctionBlock = (CarsRemoverBuildMode) control;
                this._carsRemover = new CarsRemover( roadJunctionBlock, c => context.ConductorFactory.Create( c ) );
                context.AddElement( roadJunctionBlock, this._carsRemover );
            }

            public void Connect( BuilderContext builderContext )
            {
                var connectedLane = this._carsRemover.CarsRemoverBuilder.Connector.OpositeEdge;
                if( connectedLane == null )
                {
                    return;
                }
                this._carsRemover.Lane = builderContext.GetObject<Lane>( connectedLane.Parent );
            }
        }
    }
}
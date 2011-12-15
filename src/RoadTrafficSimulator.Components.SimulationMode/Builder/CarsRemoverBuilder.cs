using System.Collections.Generic;
using System.Linq;
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
            yield return new BuilderAction( Order.Low, builder.Setup );
        }

        public bool CanCreate( IControl control )
        {
            return control != null && control.GetType() == typeof( CarsRemoverBuildMode );
        }

        public class Builder : BuilderBase
        {
            private CarsRemover _carsRemover;

            public void Build( BuilderContext context, IControl control )
            {
                var roadJunctionBlock = ( CarsRemoverBuildMode ) control;
                this._carsRemover = new CarsRemover( roadJunctionBlock, c => context.RoadInformationFactory.Create( c ) );
                context.AddElement( roadJunctionBlock, this._carsRemover );
            }

            public void Connect( BuilderContext builderContext )
            {
                var connectedLane = this._carsRemover.CarsRemoverBuilder.Connector.OpositeEdge;
                if ( connectedLane == null )
                {
                    return;
                }
                this._carsRemover.Lane = builderContext.GetObject<Lane>( connectedLane.Parent );
            }

            public void Setup( BuilderContext obj )
            {
                this._carsRemover.Routes = new StandardRoutes( Enumerable.Empty<BuildRoute>() );
            }
        }
    }
}
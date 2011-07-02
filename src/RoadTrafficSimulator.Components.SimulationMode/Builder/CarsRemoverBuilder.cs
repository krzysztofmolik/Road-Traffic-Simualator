using System.Collections.Generic;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;
using CarsRemoverBuildMode = RoadTrafficSimulator.Components.BuildMode.Controls.CarsRemover;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public class CarsRemoverBuilder : IBuilerItem
    {
        private CarsRemover _carsRemover;

        public IEnumerable<BuilderAction> Create( IControl control )
        {
            yield return new BuilderAction( Order.High, context => this.Build( context, control ) );
            yield return new BuilderAction( Order.Normal, this.Connect );
        }

        public bool CanCreate( IControl control )
        {
            return control != null && control.GetType() == typeof( CarsRemoverBuildMode );
        }

        private void Build( BuilderContext context, IControl control )
        {
            var roadJunctionBlock = ( CarsRemoverBuildMode ) control;
            this._carsRemover = new CarsRemover( roadJunctionBlock, c => new CarRemoverConductor( c ) );
            context.AddElement( roadJunctionBlock, this._carsRemover );
        }

        private void Connect( BuilderContext builderContext )
        {
            var connectedLane = this._carsRemover.CarsRemoverBuilder.Connector.OpositeEdge;
            if ( connectedLane == null ) { return; }
            this._carsRemover.Lane = builderContext.GetObject<Lane>( connectedLane.Parent );
        }
    }
}
using System.Collections.Generic;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public class LaneCornerBuilder : IBuilerItem
    {
        private LaneCorner _lane;
        public IEnumerable<BuilderAction> Create( IControl control )
        {
            yield return new BuilderAction( Order.High, context => this.Build( context, control ) );
            yield return new BuilderAction( Order.Normal, this.Connect );
        }

        public bool CanCreate( IControl control )
        {
            return control != null && control.GetType() == typeof( RoadConnection );
        }

        private void Build( BuilderContext context, IControl control )
        {
            var roadConnection = ( RoadConnection ) control;
            this._lane = new LaneCorner( roadConnection, l => context.ConductorFactory.Create( l ) );
            context.AddElement( roadConnection, this._lane );
        }

        private void Connect( BuilderContext builderContext )
        {
            this._lane.Prev = builderContext.GetObject<Lane>( this._lane.LaneCornerBuild.Connector.OpositeToPreviousEdge.Parent );
            this._lane.Next = builderContext.GetObject<Lane>( this._lane.LaneCornerBuild.Connector.OpositeToNextEdge.Parent );
        }
    }
}
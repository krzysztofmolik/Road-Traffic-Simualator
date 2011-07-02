using System.Collections.Generic;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public class RoadLaneBuilder : IBuilerItem
    {
        private Lane _lane;
        public IEnumerable<BuilderAction> Create( IControl control )
        {
            yield return new BuilderAction( Order.High, context => this.Build( context, control) );
            yield return new BuilderAction( Order.Normal, this.Connect );
        }

        public bool CanCreate( IControl control )
        {
            return control != null && control.GetType() == typeof( RoadLaneBlock );
        }

        private void Build( BuilderContext context, IControl control )
        {
            var laneBlock = (RoadLaneBlock) control;
            this._lane = new Lane( laneBlock, l => new SingleLaneConductor( l ) );
            context.AddElement( laneBlock, this._lane );
        }

        private void Connect( BuilderContext builderContext )
        {
            this._lane.Prev = builderContext.GetObject<IRoadElement>( this._lane.RoadLaneBlock.LeftEdge.Connector.PreviousEdge.Parent );
            this._lane.Next = builderContext.GetObject<IRoadElement>( this._lane.RoadLaneBlock.RightEdge.Connector.NextEdge.Parent );
        }
    }
}
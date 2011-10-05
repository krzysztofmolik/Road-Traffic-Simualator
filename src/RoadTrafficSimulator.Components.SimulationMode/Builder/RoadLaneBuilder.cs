using System;
using System.Collections.Generic;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public class RoadLaneBuilder : IBuilerItem
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
            return control != null && control.GetType() == typeof( RoadLaneBlock );
        }

        private class Builder
        {
            private Lane _lane;

            public void Build( BuilderContext context, IControl control )
            {
                var laneBlock = ( RoadLaneBlock ) control;
                this._lane = new Lane( laneBlock, l => new SingleLaneRoadInformation( l ) );
                context.AddElement( laneBlock, this._lane );
            }

            public void Connect( BuilderContext builderContext )
            {
                this._lane.Prev = builderContext.GetObject<IRoadElement>( this._lane.RoadLaneBlock.LeftEdge.Connector.PreviousEdge.Parent );
                this._lane.Next = builderContext.GetObject<IRoadElement>( this._lane.RoadLaneBlock.RightEdge.Connector.NextEdge.Parent );
            }

            public void SetUp(BuilderContext obj)
            {
//                this._lane.RoadLaneBlock.LeftEdge.Routes.CalculateProbabilities();
//                this._lane.RoadLaneBlock.RightEdge.Routes.CalculateProbabilities();
//                this._lane.RoadLaneBlock.TopEdge.Routes.CalculateProbabilities();
//                this._lane.RoadLaneBlock.BottomEdge.Routes.CalculateProbabilities();
            }
        }
    }
}
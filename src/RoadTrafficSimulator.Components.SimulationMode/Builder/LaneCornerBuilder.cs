using System;
using System.Collections.Generic;
using System.Linq;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public class LaneCornerBuilder : IBuilerItem
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
            return control != null && control.GetType() == typeof( RoadConnection );
        }

        private class Builder
        {
            private LaneCorner _lane;
            private readonly BuildRoutesToSimulationRoutesConverter _converter = new BuildRoutesToSimulationRoutesConverter();

            public void Build( BuilderContext context, IControl control )
            {
                var roadConnection = (RoadConnection) control;
                this._lane = new LaneCorner( roadConnection, l => context.RoadInformationFactory.Create( l ) );
                context.AddElement( roadConnection, this._lane );
            }

            public void Connect( BuilderContext builderContext )
            {
                this._lane.Prev = builderContext.GetObject<Lane>( this._lane.LaneCornerBuild.Connector.OpositeToPreviousEdge.Parent );
                this._lane.Next = builderContext.GetObject<Lane>( this._lane.LaneCornerBuild.Connector.OpositeToNextEdge.Parent );
            }

            public void SetUp(BuilderContext obj)
            {
//                this._lane.LaneCornerBuild.Routes.CalculateProbabilities();
                var routes = this._lane.LaneCornerBuild.Routes;
                this._lane.Routes = new StandardRoutes( this._converter.Convert( routes.AvailableRoutes, obj ).ToArray() );
            }
        }
    }
}
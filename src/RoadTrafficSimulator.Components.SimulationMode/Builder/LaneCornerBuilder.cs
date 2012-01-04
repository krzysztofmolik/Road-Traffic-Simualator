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
            yield return new BuilderAction( Order.VeryLow, builder.SetConnection );
        }

        public bool CanCreate( IControl control )
        {
            return control != null && control.GetType() == typeof( RoadConnection );
        }

        private class Builder : BuilderBase
        {
            private LaneCorner _lane;

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
//                var routes = this._lane.LaneCornerBuild.Routes;
//                var convertedRoutes = this.ConvertRoutes( routes, obj, this._lane ).ToArray();
                // TODO Remove Routes from this element
                this._lane.Routes = new StandardRoutes( Enumerable.Empty<BuildRoute>() );
            }

            public void SetConnection( BuilderContext context )
            {
                this.SetConnections( this._lane.Routes.AvailableRoutes );
            }
        }
    }
}
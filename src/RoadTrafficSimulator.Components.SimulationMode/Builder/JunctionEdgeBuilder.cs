using System.Collections.Generic;
using System.Linq;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;
using BuildJunctionEdge = RoadTrafficSimulator.Components.BuildMode.Controls.JunctionEdge;
using JunctionEdge = RoadTrafficSimulator.Components.SimulationMode.Elements.JunctionEdge;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public class JunctionEdgeBuilder : IBuilerItem
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
            return control != null && control.GetType() == typeof( BuildJunctionEdge );
        }

        private class Builder : BuilderBase
        {
            private JunctionEdge _junctionEdge;

            public void Build( BuilderContext context, IControl control )
            {
                var buildJunctionEdge = ( BuildJunctionEdge ) control;
                this._junctionEdge = new JunctionEdge( buildJunctionEdge, l => context.RoadInformationFactory.Create( l ) );
                context.AddElement( buildJunctionEdge, this._junctionEdge );
            }

            public void Connect( BuilderContext builderContext )
            {
                this._junctionEdge.Junction = builderContext.GetObject<LaneJunction>( this._junctionEdge.EdgeBuilder.Connector.JunctionEdge.Parent );
                if ( this._junctionEdge.EdgeBuilder.Connector.Edge != null )
                {
                    this._junctionEdge.Next =
                        builderContext.GetObject<IRoadElement>( this._junctionEdge.EdgeBuilder.Connector.Edge.Parent );
                }
            }

            public void SetUp( BuilderContext obj )
            {
                var routes = this._junctionEdge.EdgeBuilder.Routes;
                var convertedRoutes = this.ConvertRoutes( routes, obj ).ToArray();
                this.SetConnections( convertedRoutes, this._junctionEdge );
                this._junctionEdge.Routes = new StandardRoutes( convertedRoutes );
            }
        }
    }
}
using System.Collections.Generic;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public class LaneJunctionBuilder : IBuilerItem
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
            return control != null && control.GetType() == typeof( RoadJunctionBlock );
        }

        private class Builder
        {
            private LaneJunction _junction;

            public void Build( BuilderContext context, IControl control )
            {
                var roadJunctionBlock = (RoadJunctionBlock) control;
                this._junction = new LaneJunction( roadJunctionBlock, c => context.ConductorFactory.Create( c ) );
                context.AddElement( roadJunctionBlock, this._junction );
            }

            public void Connect( BuilderContext builderContext )
            {
                if( this._junction.JunctionBuilder.RoadJunctionEdges[ EdgeType.Bottom ].Connector.Edge != null )
                {
                    this._junction.Bottom.ConnectedEdge =
                        builderContext.GetObject<IRoadElement>(
                            this._junction.JunctionBuilder.RoadJunctionEdges[ EdgeType.Bottom ].Connector.Edge.Parent );
                }
                if( this._junction.JunctionBuilder.RoadJunctionEdges[ EdgeType.Left ].Connector.Edge != null )
                {
                    this._junction.Left.ConnectedEdge =
                        builderContext.GetObject<IRoadElement>(
                            this._junction.JunctionBuilder.RoadJunctionEdges[ EdgeType.Left ].Connector.Edge.Parent );
                }
                if( this._junction.JunctionBuilder.RoadJunctionEdges[ EdgeType.Top ].Connector.Edge != null )
                {
                    this._junction.Top.ConnectedEdge =
                        builderContext.GetObject<IRoadElement>(
                            this._junction.JunctionBuilder.RoadJunctionEdges[ EdgeType.Top ].Connector.Edge.Parent );
                }
                if( this._junction.JunctionBuilder.RoadJunctionEdges[ EdgeType.Right ].Connector.Edge != null )
                {
                    this._junction.Right.ConnectedEdge =
                        builderContext.GetObject<IRoadElement>(
                            this._junction.JunctionBuilder.RoadJunctionEdges[ EdgeType.Right ].Connector.Edge.Parent );
                }
            }

            public void SetUp( BuilderContext obj )
            {
                this._junction.Bottom.Situation.SetUp();
                this._junction.Left.Situation.SetUp();
                this._junction.Top.Situation.SetUp();
                this._junction.Right.Situation.SetUp();
                this._junction.BuildControl.VertexContainer.ReloadTextures();
            }
        }
    }
}
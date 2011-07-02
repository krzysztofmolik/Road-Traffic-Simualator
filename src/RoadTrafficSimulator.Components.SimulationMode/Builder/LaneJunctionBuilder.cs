using System.Collections.Generic;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public class LaneJunctionBuilder : IBuilerItem
    {
        private LaneJunction _junction;
        public IEnumerable<BuilderAction> Create( IControl control )
        {
            yield return new BuilderAction( Order.High, context => this.Build( context, control ) );
            yield return new BuilderAction( Order.Normal, this.Connect );
            yield return new BuilderAction( Order.Low, this.SetUp );
        }

        public bool CanCreate( IControl control )
        {
            return control != null && control.GetType() == typeof( RoadJunctionBlock );
        }

        private void Build( BuilderContext context, IControl control )
        {
            var roadJunctionBlock = ( RoadJunctionBlock ) control;
            this._junction = new LaneJunction( roadJunctionBlock, c => context.ConductorFactory.Create( c ) );
            context.AddElement( roadJunctionBlock, this._junction );
        }

        private void Connect( BuilderContext builderContext )
        {
            if ( this._junction.JunctionBuilder.RoadJunctionEdges[ EdgeType.Bottom ].Connector.Edge != null )
            {
                this._junction.Bottom.Lane = builderContext.GetObject<Lane>( this._junction.JunctionBuilder.RoadJunctionEdges[ EdgeType.Bottom ].Connector.Edge.Parent );
            }
            if ( this._junction.JunctionBuilder.RoadJunctionEdges[ EdgeType.Left ].Connector.Edge != null )
            {
                this._junction.Left.Lane = builderContext.GetObject<Lane>( this._junction.JunctionBuilder.RoadJunctionEdges[ EdgeType.Left ].Connector.Edge.Parent );
            }
            if ( this._junction.JunctionBuilder.RoadJunctionEdges[ EdgeType.Top ].Connector.Edge != null )
            {
                this._junction.Top.Lane = builderContext.GetObject<Lane>( this._junction.JunctionBuilder.RoadJunctionEdges[ EdgeType.Top ].Connector.Edge.Parent );
            }
            if ( this._junction.JunctionBuilder.RoadJunctionEdges[ EdgeType.Right ].Connector.Edge != null )
            {
                this._junction.Right.Lane = builderContext.GetObject<Lane>( this._junction.JunctionBuilder.RoadJunctionEdges[ EdgeType.Right ].Connector.Edge.Parent );
            }
        }

        private void SetUp( BuilderContext obj )
        {
            this.SetupEdge( this._junction.Bottom );
            this.SetupEdge( this._junction.Left );
            this.SetupEdge( this._junction.Top );
            this.SetupEdge( this._junction.Right );
        }

        private void SetupEdge( JunctionEdge edge )
        {
            if ( edge.Lane != null )
            {
                edge.IsOut = edge.Lane.Prev == this._junction;
            }
        }
    }
}
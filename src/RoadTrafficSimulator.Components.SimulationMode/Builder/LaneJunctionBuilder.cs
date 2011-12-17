using System.Collections.Generic;
using System.Linq;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;
using JunctionEdge = RoadTrafficSimulator.Components.SimulationMode.Elements.JunctionEdge;

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
                var roadJunctionBlock = ( RoadJunctionBlock ) control;
                this._junction = new LaneJunction( roadJunctionBlock, c => context.RoadInformationFactory.Create( c ) );
                context.AddElement( roadJunctionBlock, this._junction );
            }

            public void Connect( BuilderContext builderContext )
            {
                if ( this._junction.JunctionBuilder.Connector.LeftEdge != null )
                {
                    this._junction.Left = builderContext.GetObject<JunctionEdge>( this._junction.JunctionBuilder.Connector.LeftEdge);
                }

                if ( this._junction.JunctionBuilder.Connector.RightEdge != null )
                {
                    this._junction.Right = builderContext.GetObject<JunctionEdge>( this._junction.JunctionBuilder.Connector.RightEdge);
                }

                if ( this._junction.JunctionBuilder.Connector.TopEdge != null )
                {
                    this._junction.Top = builderContext.GetObject<JunctionEdge>( this._junction.JunctionBuilder.Connector.TopEdge);
                }
                if ( this._junction.JunctionBuilder.Connector.BottomEdge != null )
                {
                    this._junction.Bottom = builderContext.GetObject<JunctionEdge>( this._junction.JunctionBuilder.Connector.BottomEdge);
                }
            }

            public void SetUp( BuilderContext obj )
            {
                this._junction.BuildControl.VertexContainer.ReloadTextures();
                this._junction.Routes = new StandardRoutes( Enumerable.Empty<BuildRoute>() );
            }
        }
    }
}
using System;
using System.Collections.Generic;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Light;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public class LightBuilder : IBuilerItem
    {
        public IEnumerable<BuilderAction> Create( IControl control )
        {
            var builder = new Builder();
            yield return new BuilderAction( Order.High, context => builder.Build( context, control ) );
            yield return new BuilderAction( Order.Normal, builder.Connect );
        }

        public bool CanCreate( IControl control )
        {
            return control != null && control.GetType() == typeof( LightBlock );
        }

        private class Builder
        {
            private Light _light;

            public void Build( BuilderContext context, IControl control )
            {
                var lightBlock = (LightBlock) control;
                this._light = new Light( lightBlock, l => context.RoadInformationFactory.Create( l ) );
                context.AddElement( lightBlock, this._light );
            }

            public void Connect( BuilderContext builderContext )
            {
                var owner = builderContext.GetObject<LaneJunction>( this._light.LightBlock.Connector.Owner);
                this._light.Owner = owner;
                owner.AddLight( this._light );
            }
        }
    }
}
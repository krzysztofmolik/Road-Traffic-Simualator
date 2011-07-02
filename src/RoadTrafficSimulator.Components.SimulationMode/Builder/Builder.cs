using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public class Builder
    {
        private readonly IEnumerable<IBuilerItem> _builders;

        public Builder( IEnumerable<IBuilerItem> builders )
        {
            this._builders = builders;
        }

        public IEnumerable<IRoadElement> ConvertToSimulationMode( IEnumerable<IControl> controls )
        {
            var context = new BuilderContext();
            controls.Where( c => c != null )
                .SelectMany( this.GetAction )
                .OrderBy( a => a.Order )
                .ForEach( a => a.Action( context ) );

            return context.Elements;
        }

        private IEnumerable<BuilderAction> GetAction( IControl control )
        {
            var builder = this._builders.FirstOrDefault( b => b.CanCreate( control ) );
            if( builder == null ) throw new ArgumentException( "Control not supported: " + control.GetType().Name );
            return builder.Create( control );
        }
    }
}
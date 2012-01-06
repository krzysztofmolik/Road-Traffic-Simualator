using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Common;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Factories;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public class Builder
    {
        private readonly IEnumerable<IBuilerItem> _builders;
        private readonly IRoadInformationFactory _roadInformationFactory;

        public Builder( IEnumerable<IBuilerItem> builders, IRoadInformationFactory roadInformationFactory )
        {
            this._builders = builders;
            this._roadInformationFactory = roadInformationFactory;
        }

        public IEnumerable<IRoadElement> ConvertToSimulationMode( IEnumerable<IControl> controls )
        {
            Contract.Requires( controls != null );
            var context = new BuilderContext( this._roadInformationFactory );
            controls.Where( c => c != null )
                .SelectMany( this.GetAction )
                .OrderBy( a => a.Order )
                .ForEach( a => a.Action( context ) );

            return context.Elements;
        }

        private IEnumerable<BuilderAction> GetAction( IControl control )
        {
            var builder = this._builders.FirstOrDefault( b => b.CanCreate( control ) );
            if ( builder == null ) throw new ArgumentException( "Controls not supported: " + control.GetType().Name );
            return builder.Create( control );
        }
    }
}
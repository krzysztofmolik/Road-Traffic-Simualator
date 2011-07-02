using System;
using System.Collections.Generic;
using System.Linq;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors.Factories
{
    public class CondutctorFactory : IConductorFactory
    {
        private IEnumerable<IConductorFactory> _conductorFactory;

        public CondutctorFactory( IEnumerable<IConductorFactory> conductorFactory )
        {
            this._conductorFactory = conductorFactory;
        }

        public IConductor Create( IRoadElement roadElement )
        {
            var factory = this._conductorFactory.FirstOrDefault( f => f.CanCreate( roadElement ) );
            if ( factory == null ) { throw new ArgumentException( "Can't create conductor for given road element", "roadElement" ); }
            return factory.Create( roadElement );
        }

        public bool CanCreate( IRoadElement roadElement )
        {
            return this._conductorFactory.Any( s => s.CanCreate( roadElement ) );
        }
    }
}
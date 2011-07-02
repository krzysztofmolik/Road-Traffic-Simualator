using System;
using RoadTrafficSimulator.Components.BuildMode.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors.Factories
{
    public abstract class ConductorFactoryBase<T> : IConductorFactory where T : IRoadElement
    {
        public IConductor Create( IRoadElement roadElement )
        {
            return this.Create( ( T ) roadElement );
        }

        public bool CanCreate( IRoadElement roadElement )
        {
            if( roadElement == null ) throw new ArgumentNullException( "roadElement" );
            return typeof( T ) == roadElement.GetType();
        }

        protected abstract IConductor Create( T roadElemnet );
    }
}
using System;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Factories
{
    public abstract class RoadInformationsFactoryBase<T> : IRoadInformationFactory where T : IRoadElement
    {
        public IRoadInformation Create( IRoadElement roadElement )
        {
            return this.Create( ( T ) roadElement );
        }

        public bool CanCreate( IRoadElement roadElement )
        {
            if( roadElement == null ) throw new ArgumentNullException( "roadElement" );
            return typeof( T ) == roadElement.GetType();
        }

        protected abstract IRoadInformation Create( T roadElemnet );
    }
}
using System;
using RoadTrafficSimulator.Components.SimulationMode.Elements;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors.Factories
{
    public class SingleLineConductorFactory : ConductorFactoryBase<Lane>
    {
        private readonly Func<Lane, SingleLaneConductor> _conductorFactory;

        public SingleLineConductorFactory( Func<Lane, SingleLaneConductor> conductorFactory )
        {
            this._conductorFactory = conductorFactory;
        }

        protected override IConductor Create( Lane roadElemnet )
        {
            return this._conductorFactory( roadElemnet );
        }
    }
}
using System;
using RoadTrafficSimulator.Components.SimulationMode.Elements;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors.Factories
{
    public class CarInserterConductorFactory : ConductorFactoryBase<CarsInserter>
    {
        private readonly Func<CarsInserter, CarInserterConductor> _conductorFactory;

        public CarInserterConductorFactory( Func<CarsInserter, CarInserterConductor> conductorFactory )
        {
            this._conductorFactory = conductorFactory;
        }

        protected override IConductor Create( CarsInserter roadElemnet )
        {
            return this._conductorFactory( roadElemnet );
        }
    }
}
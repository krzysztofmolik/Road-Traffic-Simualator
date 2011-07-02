using System;
using RoadTrafficSimulator.Components.SimulationMode.Elements;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors.Factories
{
    public class CarRemoverConductorFactory : ConductorFactoryBase<CarsRemover>
    {
        private readonly Func<CarsRemover, CarRemoverConductor> _conductorFactory;

        public CarRemoverConductorFactory( Func<CarsRemover, CarRemoverConductor> conductorFactory )
        {
            this._conductorFactory = conductorFactory;
        }

        protected override IConductor Create( CarsRemover roadElemnet )
        {
            return this._conductorFactory( roadElemnet );
        }
    }
}
using System;
using RoadTrafficSimulator.Components.SimulationMode.Elements;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Factories
{
    public class CarInserterRoadInformationsFactory : RoadInformationsFactoryBase<CarsInserter>
    {
        private readonly Func<CarsInserter, CarInserterRoadInformation> _conductorFactory;

        public CarInserterRoadInformationsFactory( Func<CarsInserter, CarInserterRoadInformation> conductorFactory )
        {
            this._conductorFactory = conductorFactory;
        }

        protected override IRoadInformation Create( CarsInserter roadElemnet )
        {
            return this._conductorFactory( roadElemnet );
        }
    }
}
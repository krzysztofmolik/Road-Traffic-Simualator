using System;
using RoadTrafficSimulator.Components.SimulationMode.Elements;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Factories
{
    public class CarInserterRoadInformationsFactory : RoadInformationsFactoryBase<CarsInserter>
    {
        private readonly Func<CarsInserter, CarInserterRoadInformation> _roadInformationFactory;

        public CarInserterRoadInformationsFactory( Func<CarsInserter, CarInserterRoadInformation> roadInformationFactory )
        {
            this._roadInformationFactory = roadInformationFactory;
        }

        protected override IRoadInformation Create( CarsInserter roadElemnet )
        {
            return this._roadInformationFactory( roadElemnet );
        }
    }
}
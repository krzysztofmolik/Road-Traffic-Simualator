using System;
using RoadTrafficSimulator.Components.SimulationMode.Elements;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Factories
{
    public class CarRemoverRoadInformationsFactory : RoadInformationsFactoryBase<CarsRemover>
    {
        private readonly Func<CarsRemover, CarRemoverRoadInformation> _conductorFactory;

        public CarRemoverRoadInformationsFactory( Func<CarsRemover, CarRemoverRoadInformation> conductorFactory )
        {
            this._conductorFactory = conductorFactory;
        }

        protected override IRoadInformation Create( CarsRemover roadElemnet )
        {
            return this._conductorFactory( roadElemnet );
        }
    }
}
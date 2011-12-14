using System;
using RoadTrafficSimulator.Components.SimulationMode.Elements;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Factories
{
    public class SingleLineRoadInformationsFactory : RoadInformationsFactoryBase<Lane>
    {
        private readonly Func<Lane, LaneRoadInforamtion> _conductorFactory;

        public SingleLineRoadInformationsFactory( Func<Lane, LaneRoadInforamtion> conductorFactory )
        {
            this._conductorFactory = conductorFactory;
        }

        protected override IRoadInformation Create( Lane roadElemnet )
        {
            return this._conductorFactory( roadElemnet );
        }
    }
}
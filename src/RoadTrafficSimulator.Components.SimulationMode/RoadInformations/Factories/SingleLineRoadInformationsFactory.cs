using System;
using RoadTrafficSimulator.Components.SimulationMode.Elements;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Factories
{
    public class SingleLineRoadInformationsFactory : RoadInformationsFactoryBase<Lane>
    {
        private readonly Func<Lane, SingleLaneRoadInformation> _conductorFactory;

        public SingleLineRoadInformationsFactory( Func<Lane, SingleLaneRoadInformation> conductorFactory )
        {
            this._conductorFactory = conductorFactory;
        }

        protected override IRoadInformation Create( Lane roadElemnet )
        {
            return this._conductorFactory( roadElemnet );
        }
    }
}
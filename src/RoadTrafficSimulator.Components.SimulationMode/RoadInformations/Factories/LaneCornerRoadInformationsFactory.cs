using System;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Elements;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Factories
{
    public class LaneCornerRoadInformationsFactory : RoadInformationsFactoryBase<LaneCorner>
    {
        private readonly Func<LaneCorner, LaneCornerRoadInformation> _conductorFactory;

        public LaneCornerRoadInformationsFactory( Func<LaneCorner, LaneCornerRoadInformation> conductorFactory )
        {
            this._conductorFactory = conductorFactory;
        }

        protected override IRoadInformation Create( LaneCorner roadElemnet )
        {
            return this._conductorFactory( roadElemnet );
        }
    }
}
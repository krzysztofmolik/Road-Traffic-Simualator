using System;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations.LaneJunctionConductor;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Factories
{
    public class LaneJunctionRoadInformationsFactory : RoadInformationsFactoryBase<LaneJunction>
    {
        private readonly Func<LaneJunction, RoadLaneJuctionRoadInformation> _conductorFactory;

        public LaneJunctionRoadInformationsFactory( Func<LaneJunction, RoadLaneJuctionRoadInformation> conductorFactory )
        {
            this._conductorFactory = conductorFactory;
        }

        protected override IRoadInformation Create( LaneJunction roadElemnet )
        {
            return this._conductorFactory( roadElemnet );
        }
    }
}
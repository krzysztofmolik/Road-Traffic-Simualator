using System;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations.LaneJunctionConductor;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Factories
{
    public class LaneJunctionRoadInformationsFactory : RoadInformationsFactoryBase<LaneJunction>
    {
        private readonly Func<LaneJunction, RightHandRuleLaneJuctionRoadInformation> _conductorFactory;

        public LaneJunctionRoadInformationsFactory( Func<LaneJunction, RightHandRuleLaneJuctionRoadInformation> conductorFactory )
        {
            this._conductorFactory = conductorFactory;
        }

        protected override IRoadInformation Create( LaneJunction roadElemnet )
        {
            return this._conductorFactory( roadElemnet );
        }
    }
}
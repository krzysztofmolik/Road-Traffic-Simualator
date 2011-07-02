using System;
using RoadTrafficSimulator.Components.SimulationMode.Conductors.LaneJunctionConductor;
using RoadTrafficSimulator.Components.SimulationMode.Elements;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors.Factories
{
    public class LaneJunctionConductorFactory : ConductorFactoryBase<LaneJunction>
    {
        private readonly Func<LaneJunction, RightHandRuleLaneJuctionConductor> _conductorFactory;

        public LaneJunctionConductorFactory( Func<LaneJunction, RightHandRuleLaneJuctionConductor> conductorFactory )
        {
            this._conductorFactory = conductorFactory;
        }

        protected override IConductor Create( LaneJunction roadElemnet )
        {
            return this._conductorFactory( roadElemnet );
        }
    }
}
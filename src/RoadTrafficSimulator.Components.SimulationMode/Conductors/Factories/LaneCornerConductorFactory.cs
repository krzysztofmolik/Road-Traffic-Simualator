using System;
using RoadTrafficSimulator.Components.SimulationMode.Elements;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors.Factories
{
    public class LaneCornerConductorFactory : ConductorFactoryBase<LaneCorner>
    {
        private readonly Func<LaneCorner, LaneCornerConductor> _conductorFactory;

        public LaneCornerConductorFactory( Func<LaneCorner, LaneCornerConductor> conductorFactory )
        {
            this._conductorFactory = conductorFactory;
        }

        protected override IConductor Create( LaneCorner roadElemnet )
        {
            return this._conductorFactory( roadElemnet );
        }
    }
}
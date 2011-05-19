using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements
{
    public abstract class LaneConnector : RoadElementBase
    {
        protected LaneConnector( IControl control )
            : base( control )
        {
        }
    }
}
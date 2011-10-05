using RoadTrafficSimulator.Components.SimulationMode.Builder;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode
{
    public interface IRoadElement
    {
        // TODO Change name
        IControl BuildControl { get; }
        IRoadInformation RoadInformation { get; }
        IDrawer Drawer { get; }
        Routes Routes { get; }
    }
}
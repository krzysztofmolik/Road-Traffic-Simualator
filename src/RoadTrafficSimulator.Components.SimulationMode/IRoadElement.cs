using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode
{
    public interface IRoadElement
    {
        // TODO Change name
        IControl BuildControl { get; }
        IConductor Condutor { get; }
        IDrawer Drawer { get; }
    }
}
using RoadTrafficSimulator.Components.SimulationMode.Builder;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode
{
    public interface IRoadElement
    {
        // TODO Change name
        IControl BuildControl { get; }
        IDrawer Drawer { get; }
        IRoutes Routes { get; }
    }
}
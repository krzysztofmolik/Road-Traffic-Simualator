using System.Collections.Generic;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public interface IBuilerItem
    {
        IEnumerable<BuilderAction> Create( IControl control );
        bool CanCreate( IControl control );
    }
}
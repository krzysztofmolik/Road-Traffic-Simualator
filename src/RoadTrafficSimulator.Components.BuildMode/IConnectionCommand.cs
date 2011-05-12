using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode
{
    public interface IConnectionCommand
    {
        bool Connect( ILogicControl first, ILogicControl second );
    }
}
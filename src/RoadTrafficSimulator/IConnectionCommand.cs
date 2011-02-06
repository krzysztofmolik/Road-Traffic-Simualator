using RoadTrafficSimulator.Infrastructure.Control;

namespace RoadTrafficSimulator
{
    public interface IConnectionCommand
    {
        bool Connect(IControl first, IControl second);
    }
}
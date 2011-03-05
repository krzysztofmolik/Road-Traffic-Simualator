using RoadTrafficSimulator.Infrastructure.Control;

namespace RoadTrafficSimulator
{
    public interface IConnectionCommand
    {
        bool Connect(ILogicControl first, ILogicControl second);
    }
}
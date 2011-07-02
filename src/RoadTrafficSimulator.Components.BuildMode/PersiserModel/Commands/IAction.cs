using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands
{
    public interface IAction
    {
        void Execute(DeserializationContext context);
        Order Priority { get; }
    }
}
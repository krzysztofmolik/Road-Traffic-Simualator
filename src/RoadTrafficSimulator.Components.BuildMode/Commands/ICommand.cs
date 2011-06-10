namespace RoadTrafficSimulator.Components.BuildMode.Commands
{
    public interface ICommand
    {
        CommandType CommandType { get; }
        void Start();
        void Stop();
    }
}
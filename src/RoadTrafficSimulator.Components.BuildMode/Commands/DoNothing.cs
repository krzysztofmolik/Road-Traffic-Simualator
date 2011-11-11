namespace RoadTrafficSimulator.Components.BuildMode.Commands
{
    public class DoNothing : ICommand
    {
        public CommandType CommandType
        {
            get { return CommandType.Clear; }
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }
    }
}
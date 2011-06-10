using RoadTrafficSimulator.Components.BuildMode.Commands;

namespace RoadTrafficSimulator.Components.BuildMode.Messages
{
    public class ExecuteCommand
    {
        public ExecuteCommand( CommandType commandType )
        {
            this.CommandType = commandType;
        }

        public CommandType CommandType { get; private set; }
    }
}
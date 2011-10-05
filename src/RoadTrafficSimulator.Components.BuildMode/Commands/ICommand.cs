namespace RoadTrafficSimulator.Components.BuildMode.Commands
{
    public class GuiCommand
    {
        public GuiCommand(GuiCommandType commandType)
        {
            CommandType = commandType;
        }

        public GuiCommandType CommandType { get; private set; }
        public object Paramter { get; set; }
    }

    public enum GuiCommandType
    {
        EditSelected,
    }

    public interface ICommand
    {
        CommandType CommandType { get; }
        void Start();
        void Stop();
    }
}
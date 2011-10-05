using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Commands
{
    public class GuiCommdnEdit
    {
        public GuiCommdnEdit(IControl control)
        {
            this.Control = control;
        }

        public IControl Control { get; private set; }
    }
}
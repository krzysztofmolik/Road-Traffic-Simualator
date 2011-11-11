using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Commands
{
    public class GuiCommdnControlClicked
    {
        public GuiCommdnControlClicked(IControl control)
        {
            this.Control = control;
        }

        public IControl Control { get; private set; }
    }
}
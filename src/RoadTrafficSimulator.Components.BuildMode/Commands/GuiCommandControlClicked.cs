using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Commands
{
    public class GuiCommandControlClicked
    {
        public GuiCommandControlClicked(IControl control)
        {
            this.Control = control;
        }

        public IControl Control { get; private set; }
    }
}
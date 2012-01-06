using System.Collections.Generic;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Commands
{
    public class GuiCommdnEdit
    {
        public GuiCommdnEdit(IEnumerable<IControl> control)
        {
            this.Controls = control;
        }

        public IEnumerable<IControl> Controls { get; private set; }
    }
}
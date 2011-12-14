using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class NewControlCreated
    {
        public NewControlCreated( IControl control )
        {
            Control = control;
        }

        public IControl Control { get; private set; }
    }
}
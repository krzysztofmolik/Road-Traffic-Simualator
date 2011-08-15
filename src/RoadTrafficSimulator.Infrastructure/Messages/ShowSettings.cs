using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Infrastructure.Messages
{
    public class ShowSettings
    {
        public ShowSettings( IControl control )
        {
            this.Control = control;
        }

        public IControl Control { get; private set; }
    }
}
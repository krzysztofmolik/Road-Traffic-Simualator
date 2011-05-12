using System.Diagnostics.Contracts;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Infrastructure.Messages
{
    public class AddControlToRoadLayer
    {
        public AddControlToRoadLayer( IControl control )
        {
            Contract.Requires( control != null );
            this.Control = control;
        }

        public IControl Control { get; private set; }
    }
}
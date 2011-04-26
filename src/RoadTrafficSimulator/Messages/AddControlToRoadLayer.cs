using System.Diagnostics.Contracts;
using RoadTrafficSimulator.Infrastructure.Control;

namespace RoadTrafficSimulator.Messages
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
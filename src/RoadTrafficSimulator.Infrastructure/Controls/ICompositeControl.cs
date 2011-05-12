using System.Collections.Generic;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Mouse;

namespace RoadTrafficSimulator.Road
{
    public interface ICompositeControl : IControl
    {
        IEnumerable<IControl> Children { get; }

        void AddChild( IControl control );
    }
}
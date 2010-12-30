using System.Collections.Generic;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;

namespace RoadTrafficSimulator.Road
{
    public interface ICompostControlBase : IControl
    {
        IEnumerable<IControl> Children { get; }
        IConnectionCompositeSupport ConnectionSupport { get; }
        void AddChild( IControl singleControlBase );
    }
}
using System.Collections.Generic;

namespace RoadTrafficSimulator.Infrastructure.Controls
{
    public interface ICompositeControl : IControl
    {
        IEnumerable<IControl> Children { get; }

        void AddChild( IControl control );
    }
}
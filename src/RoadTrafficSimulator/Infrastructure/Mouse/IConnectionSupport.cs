using System.Collections.Generic;
using RoadTrafficSimulator.Infrastructure.Control;

namespace RoadTrafficSimulator.Infrastructure.Mouse
{
    public interface IConnectionSupport
    {
        IControl Owner { get; }
        IEnumerable<IConnectionSupport> ConnectedObject { get; }
        void Connect( IConnectionSupport objectToConnect );
    }

    public interface IConnectionCompositeSupport : IConnectionSupport
    {
        void ConnectChildren( IControl children, IConnectionSupport objectToConnect );
    }
}
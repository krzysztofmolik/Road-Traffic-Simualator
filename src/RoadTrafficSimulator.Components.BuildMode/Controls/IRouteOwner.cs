using System.Collections.Generic;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public interface IRouteOwner : IControl
    {
        Routes Routes { get; }
    }

}
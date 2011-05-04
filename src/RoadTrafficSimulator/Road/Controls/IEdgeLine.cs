using System;
using RoadTrafficSimulator.Infrastructure.Control;

namespace RoadTrafficSimulator.Road.Controls
{
    public interface IEdgeLine : IEdge
    {
        void RecalculatePostitionAroundStartPoint();
        void RecalculatePostitionAroundEndPoint();
        void RecalculatePosition();
    }
}
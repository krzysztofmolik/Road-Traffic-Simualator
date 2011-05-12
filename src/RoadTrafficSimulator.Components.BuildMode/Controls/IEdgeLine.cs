namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public interface IEdgeLine : IEdge
    {
        void RecalculatePostitionAroundStartPoint();
        void RecalculatePostitionAroundEndPoint();
        void RecalculatePosition();
    }
}
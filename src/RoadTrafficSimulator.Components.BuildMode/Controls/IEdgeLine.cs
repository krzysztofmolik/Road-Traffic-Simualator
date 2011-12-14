namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public interface IEdgeLine
    {
        void RecalculatePostitionAroundStartPoint();
        void RecalculatePostitionAroundEndPoint();
        void RecalculatePosition();
    }
}
using Microsoft.Xna.Framework;

namespace RoadTrafficSimulator.Infrastructure.Messages
{
    public class ChangedToBuildMode
    {}

    public class ChangedToSimulationMode { } 

    public class ChangedZoom
    {
        public ChangedZoom( float percent )
        {
            this.Percent = percent;
        }

        public float Percent { get; private set; }
    }

    public class CenterPointChanged
    {
        public CenterPointChanged( Vector2 centerPoint )
        {
            this.CenterPoint = centerPoint;
        }

        public Vector2 CenterPoint { get; set; }
    }

}
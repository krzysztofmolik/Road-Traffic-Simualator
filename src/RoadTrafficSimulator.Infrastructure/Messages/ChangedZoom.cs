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
}
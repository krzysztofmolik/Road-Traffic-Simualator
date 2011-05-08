namespace RoadTrafficSimulator.Messages
{
    public class ChangeZoomMessage
    {
        public ChangeZoomMessage( float percent )
        {
            this.Percent = percent;
        }

        public float Percent { get; private set; }
    }
}
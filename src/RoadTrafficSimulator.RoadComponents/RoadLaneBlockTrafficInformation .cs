using RoadTrafficSimulator.Road;

namespace RoadTrafficSimulator.RoadComponents
{
    public class RoadLaneBlockTrafficInformation : IRoadElement
    {
        public RoadLaneBlockTrafficInformation( IRoadLaneBlock roadLaneBlock )
        {
            this.RoadLaneBlock = roadLaneBlock;
        }

        public IRoadLaneBlock RoadLaneBlock { get; private set; }
    }
}

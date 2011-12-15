using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public class RouteElement
    {
        public IRoadElement RoadElement { get; set; }
        public PriorityType PriorityType { get; set; }
        public bool CanStopOnIt { get; set; }
    }
}
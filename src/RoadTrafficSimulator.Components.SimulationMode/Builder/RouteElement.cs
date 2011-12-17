using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    public class RouteElement
    {
        private static RouteElement _empty = new RouteElement();
        public static RouteElement Empty { get { return _empty; } }

        public IRoadElement RoadElement { get; set; }
        public PriorityType PriorityType { get; set; }
        public bool CanStopOnIt { get; set; }
        public float Length { get; set; }
    }
}
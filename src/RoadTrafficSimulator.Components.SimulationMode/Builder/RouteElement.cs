using System.Diagnostics;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.SimulationMode.Builder
{
    [DebuggerDisplay("DebugerInfo()")]
    public class RouteElement
    {
        private static RouteElement _empty = new RouteElement();
        public static RouteElement Empty { get { return _empty; } }

        public IRoadElement RoadElement { get; set; }
        public PriorityType PriorityType { get; set; }
        public bool CanStopOnIt { get; set; }
        public float Length { get; set; }

        private string DebugerInfo()
        {
            return string.Format( "Road element {0}, Priority {1}", this.RoadElement.GetType().Name, this.PriorityType );
        }
    }
}
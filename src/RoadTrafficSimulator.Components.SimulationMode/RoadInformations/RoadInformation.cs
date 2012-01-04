using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations
{
    public class RoadInformation
    {
        private static RoadInformation _empty = new RoadInformation
                                                    {
                                                        CanDriver = true,
                                                        CarAhead = null,
                                                        CarAheadDistance = float.MaxValue,
                                                        PrivilagesCarInformation = new PriorityInformation[ 0 ],
                                                        CanStop = true,
                                                    };

        public static RoadInformation Empty { get { return _empty; } }
        public Car CarAhead { get; set; }
        public float CarAheadDistance { get; set; }
        public bool CanDriver { get; set; }
        public bool CanStop { get; set; }

        public PriorityInformation[] PrivilagesCarInformation { get; set; }
    }
}
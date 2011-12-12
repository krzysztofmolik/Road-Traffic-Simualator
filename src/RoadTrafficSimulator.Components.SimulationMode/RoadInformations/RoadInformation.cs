using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations
{
    public class RoadInformation
    {
        private static RoadInformation _empty = new RoadInformation
                                                    {
                                                        CanDriver = true,
                                                        CarAhead = null,
                                                        CarAheadDistance = float.MaxValue,
                                                        PrivilagesCar = null,
                                                        PrivilagesCarDistance = float.MaxValue,
                                                        StopPoint = float.MaxValue
                                                    };
        public static RoadInformation Empty { get { return _empty; } }

        public Car CarAhead { get; set; }
        public float CarAheadDistance { get; set; }
        public bool CanDriver { get; set; }
        public float StopPoint { get; set; }

        public Car PrivilagesCar { get; set; }
        public float PrivilagesCarDistance { get; set; }
    }
}
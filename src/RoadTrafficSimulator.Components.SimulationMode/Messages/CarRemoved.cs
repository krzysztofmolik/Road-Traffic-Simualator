using System.Diagnostics.Contracts;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.Messages
{
    public class CarRemoved
    {
        public CarRemoved( Car car )
        {
            Contract.Requires( car != null );
            this.Car = car;
        }

        public Car Car { get; private set; }
    }
}
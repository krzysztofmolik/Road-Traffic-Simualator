using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.SimulationMode.CarsSpecification
{
    public class PassengerCarFactory : ICarSpecifiaction
    {
        private const float Velocity = 60.0f;
        private const float Widht = 2.5f;
        private const float Length = 4.0f;

        public Car Create()
        {
            var car = new Car()
                          {
                              Velocity = this.ToVirtualUnitSpeed( Velocity ),
                              Width = Constans.ToVirtualUnit( Widht ),
                              Lenght = Constans.ToVirtualUnit( Length ),
                          };
            return car;
        }

        private float ToVirtualUnitSpeed( float kmPerHour )
        {
            var unitPerHour = Constans.KmToVirtualUnit( kmPerHour );
            return unitPerHour / Constans.MsPerHour;
        }
    }
}
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.SimulationMode.CarsSpecification
{
    public class PassengerCarFactory : ICarSpecifiaction
    {
        private const float Widht = 2.5f;
        private const float Length = 4.0f;

        public Car Create()
        {
            var car = new Car()
                          {
                              Width = UnitConverter.FromMeter( Widht ),
                              Lenght = UnitConverter.FromMeter( Length ),
                              BreakingForce = UnitConverter.FromKmPerHour( 10.0f ) / UnitConverter.FromSecond( 1.0f ),
                              AccelerateForce = UnitConverter.FromKmPerHour( 5.0f ) / UnitConverter.FromSecond( 1.0f ),
                              MaxSpeed = this.ToVirtualUnitSpeed( 60.0f ),
                              Velocity = 0.0f,
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
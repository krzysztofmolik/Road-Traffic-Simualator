using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations
{
    public abstract class RoadInformationBase
    {
        protected CarsQueue Cars = new CarsQueue();

        public CarAhedInformation GetCarAheadDistance( Car car )
        {
            if ( this.Cars.Contains( car ) )
            {
                var carAhed = this.Cars.GetCarAheadOf( car );
                if ( carAhed != null )
                {
                    return new CarAhedInformation
                               {
                                   CarAhead = carAhed,
                                   CarDistance = Vector2.Distance( car.Location, carAhed.Location ),
                               };
                }

                return CarAhedInformation.Empty;
            }

            var firstCar = this.Cars.GetFirstCar();
            if ( firstCar != null )
            {
                return new CarAhedInformation
                           {
                               CarAhead = firstCar,
                               CarDistance = Vector2.Distance( this.GetEndLocation(), firstCar.Location ),
                           };
            }

            return CarAhedInformation.Empty;
        }

        public virtual void OnEnter( Car car )
        {
            Contract.Requires( car != null );
            this.Cars.Add( car );
        }

        public void OnExit( Car car )
        {
            this.Cars.Remove( car );
        }

        protected abstract Vector2 GetEndLocation();
    }
}
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Route;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations
{
    public abstract class RoadInformationBase
    {
        protected CarsQueue Cars = new CarsQueue();
        private readonly List<IRoadElement> _reversConnection = new List<IRoadElement>();
        private readonly List<IRoadElement> _connections = new List<IRoadElement>();

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
                               CarDistance = Vector2.Distance( this.GetBeginLocation(), firstCar.Location ),
                           };
            }

            return CarAhedInformation.Empty;
        }

        public bool ContainsCar( Car car )
        {
            return this.Cars.Contains( car );
        }


        public FirstCarToOutInformation GetFirstCarToOutInformation()
        {
            var firstCar = this.Cars.GetFirstCar();
            if ( firstCar != null )
            {
                return new FirstCarToOutInformation( firstCar, Vector2.Distance( firstCar.Location, this.GetEndLocation() ) );
            }
            return FirstCarToOutInformation.Empty;
        }

        protected abstract Vector2 GetEndLocation();

        public void SetConnection( IRoadElement roadElement )
        {
            if ( roadElement == null ) { return; }
            this._connections.Add( roadElement );
        }

        public void SetReversConnection( IRoadElement roadElement )
        {
            if ( roadElement == null ) { return; }
            this._reversConnection.Add( roadElement );
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

        protected abstract Vector2 GetBeginLocation();
    }
}
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations
{
    public class CarInserterRoadInformation :RoadInformationBase, IRoadInformation
    {
        private readonly CarsInserter _carInserter;
        private readonly CarsQueue _cars = new CarsQueue();

        public CarInserterRoadInformation( CarsInserter carInserter )
        {
            Contract.Requires( carInserter != null );
            this._carInserter = carInserter;
        }

        protected override Vector2 GetBeginLocation()
        {
            return this._carInserter.BuildControl.Location;
        }

        public float Lenght( IRoadElement previous, IRoadElement next )
        {
            return Constans.PointSize;
        }

        public bool IsCarPresent( Car car )
        {
            return this._cars.Contains( car );
        }

        public bool ShouldChange( Car car )
        {
            return true;
        }

        public FirstCarToOutInformation GetFirstCarToOutInformation()
        {
            var firstCar = this._cars.GetFirstCar();
            if ( firstCar != null )
            {
                carInformation.Add( firstCar, carInformation.CurrentDistance + Constans.PointSize );
            }
        }

        public Vector2 GetCarDirection( Car car, IRoadElement nextPoint )
        {
            //            return this._carInserter.Lane.RoadInformation.GetCarDirection( car );
            return Vector2.Zero;
        }

        public float GetCarDistanceTo( Car car, IRoadElement nextPoint )
        {
            throw new System.NotImplementedException();
        }
    }
}

using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors
{
    public class CarInserterRoadInformation : IRoadInformation
    {
        private readonly CarsInserter _carInserter;
        private readonly CarsQueue _cars = new CarsQueue();

        public CarInserterRoadInformation( CarsInserter carInserter )
        {
            Contract.Requires( carInserter != null );
            this._carInserter = carInserter;
        }

        public void OnEnter( Car car )
        {
            this._cars.Add( car );
        }

        public void OnExit( Car car )
        {
            this._cars.Remove( car );
        }

        public float Lenght(IRoadElement previous, IRoadElement next)
        {
            return Constans.PointSize;
        }

        public bool IsCarPresent( Car car )
        {
            return this._cars.Contains( car );
        }

        public bool ShouldChange(Car car)
        {
            return true;
        }

        public void GetCarAheadDistance( FirstCarToOutInformation carInformation )
        {
//            var carAhead = this._cars.GetCarAheadOf( carInformation.QuestioningCar );
//            if ( carAhead == null )
//            {
//                carInformation.CarDistance += Constans.PointSize;
//                routMark.MoveNext();
//                routMark.Current.Condutor.GetCarAheadDistance( routMark, carInformation );
//            }
//            else
//            {
//                carInformation.QuestioningCar = carAhead;
//            }
//
//            return;
        }

        public void GetFirstCarToOutInformation( FirstCarToOutInformation carInformation )
        {
            var firstCar = this._cars.GetFirstCar();
            if ( firstCar != null )
            {
                carInformation.Add( firstCar, carInformation.CurrentDistance + Constans.PointSize );
            }
        }

        public Vector2 GetCarDirection( Car car )
        {
//            return this._carInserter.Lane.RoadInformation.GetCarDirection( car );
            return Vector2.Zero;
        }
    }
}

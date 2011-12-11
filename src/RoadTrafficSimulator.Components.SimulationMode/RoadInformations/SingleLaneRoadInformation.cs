using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Route;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations
{
    public class SingleLaneRoadInformation : IRoadInformation
    {
        private readonly Lane _lane;
        private readonly CarsQueue _cars = new CarsQueue();

        public SingleLaneRoadInformation( Lane lane )
        {
            Contract.Requires( lane != null );
            this._lane = lane;
        }

        public void OnEnter( Car car )
        {
            Contract.Requires( car != null );
            this._cars.Add( car );
        }

        public void OnExit( Car car )
        {
            this._cars.Remove( car );
        }

        public float Lenght( IRoadElement previous, IRoadElement next )
        {
            if ( this._lane.Prev != previous || this._lane.Next != next )
            {
                throw new NotImplementedException();
            }

            return Vector2.Distance( this._lane.Prev.BuildControl.Location, this._lane.Next.BuildControl.Location );
        }

        public bool CanStop( IRoadElement previous, IRoadElement next )
        {
            return true;
        }

        public bool ShouldChange( Car car )
        {
            var distance = this._lane.RoadLaneBlock.RightEdge.Location - car.Location;
            // TODO:
            if ( distance.Length() <= 0.001f ) { return true; }

            return Math.Sign( distance.X ) != Math.Sign( car.Direction.X ) && Math.Sign( distance.Y ) != Math.Sign( car.Direction.Y );
        }

        public void GetCarAheadDistance( IRouteMark<IRoadElement> routMark, CarInformation carInformation )
        {
            throw new NotImplementedException();
        }

        public float GetDistanceToStopLine()
        {
            return float.MaxValue;
        }

        public void GetFirstCarToOutInformation( FirstCarToOutInformation carInformation )
        {
            var firstCar = this._cars.GetFirstCar();
            if ( firstCar != null )
            {
                var carDistance = carInformation.CurrentDistance + Vector2.Distance( firstCar.Location, this._lane.RoadLaneBlock.RightEdge.Location );
                carInformation.Add( firstCar, carDistance );
            }
            else
            {
                carInformation.CurrentDistance += Vector2.Distance( this._lane.RoadLaneBlock.LeftEdge.Location, this._lane.RoadLaneBlock.RightEdge.Location );
                //                this._lane.Prev.RoadInformation.GetFirstCarToOutInformation( carInformation );
            }
        }

        public Vector2 GetCarDirection( Car car, IRoadElement nextPoint )
        {
            return this._lane.RoadLaneBlock.RightEdge.Location - car.Location;
        }

        public float GetCarDistanceTo( Car car, IRoadElement nextPoint )
        {
            Debug.Assert( this._lane.Next == nextPoint );

            return Vector2.Distance( car.Location, nextPoint.BuildControl.Location );
        }
    }
}
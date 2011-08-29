using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Route;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors
{
    public class SingleLaneConductor : IConductor
    {
        private readonly Lane _lane;
        private readonly CarsQueue _cars = new CarsQueue();

        public SingleLaneConductor( Lane lane )
        {
            Contract.Requires( lane != null );
            this._lane = lane;
        }

        public IRoadElement GetNextRandomElement( List<IRoadElement> route, Random rng )
        {
            Debug.Assert( this._lane.Top == null );
            Debug.Assert( this._lane.Bottom == null );
            return this._lane.Next;
        }

        public void Take( Car car )
        {
            Contract.Requires( car != null );
            this._cars.Add( car );
        }

        public void Remove( Car car )
        {
            this._cars.Remove( car );
        }

        public float GetCarDistanceToEnd( Car car )
        {
            if ( this._cars.Contains( car ) == false ) { return float.MaxValue; }

            return Vector2.Distance( car.Location, this._lane.RoadLaneBlock.RightEdge.Location );
        }

        public bool IsPosibleToDriveFrom( IRoadElement roadElement )
        {
            return this._lane.Prev == roadElement || this._lane.Top == roadElement || this._lane.Bottom == roadElement;
        }

        public bool IsPosibleToDriveTo( IRoadElement roadElement )
        {
            return this._lane.Next == roadElement || this._lane.Top == roadElement || this._lane.Bottom == roadElement;
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

        public bool ShouldChange( Vector2 acutalCarLocation, Car car )
        {
            var distance = this._lane.RoadLaneBlock.RightEdge.Location - acutalCarLocation;
            // TODO Check value and extract some kind of property
            if ( distance.Length() <= 0.001f ) { return true; }

            return Math.Sign( distance.X ) != Math.Sign( car.Direction.X ) && Math.Sign( distance.Y ) != Math.Sign( car.Direction.Y );
        }

        public float GetDistanceToStopLine()
        {
            return float.MaxValue;
        }

        public void GetLightInformation( IRouteMark routeMark, LightInfomration lightInformation )
        {
            routeMark.MoveNext();
            routeMark.Current.Condutor.GetLightInformation( routeMark, lightInformation );
        }

        public void GetNextJunctionInformation( IRouteMark route, JunctionInformation junctionInformation )
        {
            junctionInformation.JunctionDistance += Vector2.Distance( this._lane.RoadLaneBlock.LeftEdge.Location, this._lane.RoadLaneBlock.RightEdge.Location );

            route.MoveNext();
            route.Current.Condutor.GetNextJunctionInformation( route, junctionInformation );
        }

        public void GetCarAheadDistance( IRouteMark routMark, CarInformation carInformation )
        {
            if ( this._cars.Contains( carInformation.QuestioningCar ) )
            {
                var carAhead = this._cars.GetCarAheadOf( carInformation.QuestioningCar );
                if ( carAhead != null )
                {
                    carInformation.CarDistance += Vector2.Distance( carInformation.QuestioningCar.Location, carAhead.Location );
                    carInformation.CarAhead = carAhead;
                }
                else
                {

                    carInformation.CarDistance += Vector2.Distance( this._lane.RoadLaneBlock.RightEdge.Location, carInformation.QuestioningCar.Location );
                    routMark.MoveNext();
                    routMark.Current.Condutor.GetCarAheadDistance( routMark, carInformation );
                }
            }
            else
            {
                var cA = this._cars.GetFirstCar();
                if ( cA != null )
                {
                    carInformation.CarDistance += Vector2.Distance( cA.Location, this._lane.RoadLaneBlock.LeftEdge.Location );
                    carInformation.CarAhead = cA;
                }
                else
                {
                    carInformation.CarDistance += Vector2.Distance( this._lane.RoadLaneBlock.LeftEdge.Location, this._lane.RoadLaneBlock.RightEdge.Location );
                    routMark.MoveNext();
                    routMark.Current.Condutor.GetCarAheadDistance( routMark, carInformation );
                }
            }
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
                this._lane.Prev.Condutor.GetFirstCarToOutInformation( carInformation );
            }
        }

        public Vector2 GetCarDirection( Car car )
        {
            return this._lane.RoadLaneBlock.RightEdge.Location - car.Location;
        }
    }
}
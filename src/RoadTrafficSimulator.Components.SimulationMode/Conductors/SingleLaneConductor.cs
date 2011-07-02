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

        public IRoadElement GetNextRandomElement( List<IRoadElement> route )
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
            routeMark.GetNext().Condutor.GetLightInformation( routeMark.MoveNext(), lightInformation );
        }

        public void GetNextJunctionInformation( RouteMark route, JunctionInformation junctionInformation )
        {
            if( this._cars.Contains( junctionInformation.Car ))
            {
                junctionInformation.JunctionDistance += Vector2.Distance( junctionInformation.Car.Location, this._lane.RoadLaneBlock.RightEdge.Location );
            }
            else
            {
                junctionInformation.JunctionDistance += Vector2.Distance( this._lane.RoadLaneBlock.LeftEdge.Location, this._lane.RoadLaneBlock.RightEdge.Location ):
            }

            route.MoveNext();
            route.Current.Condutor.GetNextJunctionInformation( route, junctionInformation );
        }

        public void GetCarAheadDistance( IRouteMark routMark, CarInformation carInformation )
        {
            if ( this._cars.Contains( carInformation.Car ) )
            {
                var carAhead = this._cars.GetCarAheadOf( carInformation.Car );
                if ( carAhead != null )
                {
                    carInformation.CarDistance += Vector2.Distance( carInformation.Car.Location, carAhead.Location );
                    carInformation.CarAhead = carAhead;
                }
                else
                {

                    carInformation.CarDistance += Vector2.Distance( this._lane.RoadLaneBlock.RightEdge.Location, carInformation.Car.Location );
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

        public Vector2 GetCarDirection( Car car )
        {
            return this._lane.RoadLaneBlock.RightEdge.Location - car.Location;
        }
    }
}
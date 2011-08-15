using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using RoadTrafficSimulator.Infrastructure;
using System.Linq;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors
{
    public class LaneCornerConductor : IConductor
    {
        private readonly LaneCorner _laneCorner;
        private readonly CarsQueue _cars = new CarsQueue();

        public LaneCornerConductor( LaneCorner laneCorner )
        {
            Contract.Requires( laneCorner != null );
            this._laneCorner = laneCorner;
        }

        public IRoadElement GetNextRandomElement( List<IRoadElement> route, Random rng )
        {
            return this._laneCorner.Next;
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
            if ( this._cars.Contains( car ) )
            {
                return Constans.PointSize;
            }

            return float.MaxValue;
        }

        public bool IsPosibleToDriveFrom( IRoadElement roadElement )
        {
            return this._laneCorner.Prev == roadElement;
        }

        public bool IsPosibleToDriveTo( IRoadElement roadElement )
        {
            return this._laneCorner.Next == roadElement;
        }

        public void GetNextAvailablePointToStop( IRouteMark route, NextAvailablePointToStopInfo info )
        {
            route.MoveNext();
            info.Length += Constans.PointSize;
            route.Current.Condutor.GetNextAvailablePointToStop( route, info );
        }

        public bool ShouldChange( Vector2 acutalCarLocation, Car car )
        {
            var distance = this._laneCorner.BuildControl.Location - acutalCarLocation;
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
            junctionInformation.JunctionDistance += Constans.PointSize;
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
                    return;
                }
            }
            else
            {
                var cA = this._cars.GetFirstCar();
                if ( cA != null )
                {
                    carInformation.CarDistance += Vector2.Distance( this._laneCorner.LaneCornerBuild.LeftEdge.Location, cA.Location );
                    carInformation.CarAhead = cA;
                    return;
                }
            }

            carInformation.CarDistance += Vector2.Distance( this._laneCorner.LaneCornerBuild.LeftEdge.Location, this._laneCorner.LaneCornerBuild.RightEdge.Location );
            routMark.MoveNext();
            routMark.Current.Condutor.GetCarAheadDistance( routMark, carInformation );
        }

        public void GetFirstCarToOutInformation( FirstCarToOutInformation carInformation )
        {
            var firstCar = this._cars.GetFirstCar();
            if ( firstCar != null )
            {
                carInformation.Add( firstCar, carInformation.CurrentDistance + Constans.PointSize );
            }
            else
            {
                carInformation.CurrentDistance += Constans.PointSize;
                this._laneCorner.Prev.Condutor.GetFirstCarToOutInformation( carInformation );
            }
        }

        public Vector2 GetCarDirection( Car car )
        {
            return this._laneCorner.Next.Condutor.GetCarDirection( car );
        }
    }
}
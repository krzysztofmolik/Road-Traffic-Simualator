using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations
{
    public class LaneCornerRoadInformation : IRoadInformation
    {
        private readonly LaneCorner _laneCorner;
        private readonly CarsQueue _cars = new CarsQueue();

        public LaneCornerRoadInformation( LaneCorner laneCorner )
        {
            Contract.Requires( laneCorner != null );
            this._laneCorner = laneCorner;
        }

        public IRoadElement GetNextRandomElement( List<IRoadElement> route, Random rng )
        {
            return this._laneCorner.Next;
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

        public float Lenght(IRoadElement previous, IRoadElement next)
        {
            return Constans.PointSize;
        }

        public bool CanStop(IRoadElement previous, IRoadElement next)
        {
            return true;
        }

        public bool ShouldChange(Car car)
        {
            var distance = this._laneCorner.BuildControl.Location - acutalCarLocation;
            // TODO Check value and extract some kind of property
            if ( distance.Length() <= 0.001f ) { return true; }

            return Math.Sign( distance.X ) != Math.Sign( car.Direction.X ) && Math.Sign( distance.Y ) != Math.Sign( car.Direction.Y );
        }

        public void GetCarAheadDistance( IRouteMark<IRoadElement> routMark, CarInformation carInformation )
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
            routMark.Current.RoadInformation.GetCarAheadDistance( routMark, carInformation );
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
                this._laneCorner.Prev.RoadInformation.GetFirstCarToOutInformation( carInformation );
            }
        }

        public Vector2 GetCarDirection( Car car )
        {
            return this._laneCorner.Next.RoadInformation.GetCarDirection( car );
        }
    }
}
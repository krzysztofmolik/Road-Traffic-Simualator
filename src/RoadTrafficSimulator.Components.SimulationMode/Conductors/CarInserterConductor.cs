using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors
{
    public class CarInserterConductor : IConductor
    {
        private readonly CarsInserter _carInserter;
        private readonly CarsQueue _cars = new CarsQueue();

        public CarInserterConductor( CarsInserter carInserter )
        {
            Contract.Requires( carInserter != null );
            this._carInserter = carInserter;
        }

        public IRoadElement GetNextRandomElement( List<IRoadElement> route, Random rng )
        {
            return this._carInserter.Lane;
        }

        public void Take( Car car )
        {
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
            return false;
        }

        public bool IsPosibleToDriveTo( IRoadElement roadElement )
        {
            return this._carInserter.Lane == roadElement;
        }

        public float Lenght(IRoadElement previous, IRoadElement next)
        {
            return Constans.PointSize;
        }

        public bool CanStop(IRoadElement previous, IRoadElement next)
        {
            return true;
        }

        public bool IsCarPresent( Car car )
        {
            return this._cars.Contains( car );
        }

        public bool ShouldChange( Vector2 acutalCarLocation, Car car )
        {
            return true;
        }

        public float GetDistanceToStopLine()
        {
            return float.MaxValue;
        }

        public void GetLightInformation( IRouteMark routeMark, LightInfomration lightInformation )
        {
            // TODO Fix it
            lightInformation.LightDistance = float.MaxValue;
        }

        public void GetNextJunctionInformation( IRouteMark route, JunctionInformation junctionInformation )
        {
            junctionInformation.JunctionDistance += Constans.PointSize;
            route.MoveNext();
            route.Current.Condutor.GetNextJunctionInformation( route, junctionInformation );
        }

        public void GetCarAheadDistance( IRouteMark routMark, CarInformation carInformation )
        {
            var carAhead = this._cars.GetCarAheadOf( carInformation.QuestioningCar );
            if ( carAhead == null )
            {
                carInformation.CarDistance += Constans.PointSize;
                routMark.MoveNext();
                routMark.Current.Condutor.GetCarAheadDistance( routMark, carInformation );
            }
            else
            {
                carInformation.QuestioningCar = carAhead;
            }

            return;
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
            return this._carInserter.Lane.Condutor.GetCarDirection( car );
        }
    }
}
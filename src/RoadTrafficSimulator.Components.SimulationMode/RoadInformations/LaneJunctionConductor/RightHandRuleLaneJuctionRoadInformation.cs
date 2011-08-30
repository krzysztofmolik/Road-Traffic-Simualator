using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Route;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.LaneJunctionConductor
{
    public class RightHandRuleLaneJuctionRoadInformation : IRoadInformation
    {
        private readonly LaneJunction _laneJunction;

        private readonly LaneJunctionConductorCarsInformation _carsInformation;
        private readonly LaneJunctionConductorMoveInfomation _moveInformation;
        private readonly LaneJucntionConductorRightHandJunctionInformation _junctionInformation;

        public RightHandRuleLaneJuctionRoadInformation( LaneJunction laneJunction )
        {
            Contract.Requires( laneJunction != null );
            this._laneJunction = laneJunction;
            this._carsInformation = new LaneJunctionConductorCarsInformation( this._laneJunction );
            this._moveInformation = new LaneJunctionConductorMoveInfomation( this._laneJunction );
            this._junctionInformation = new LaneJucntionConductorRightHandJunctionInformation( this._laneJunction );
        }

        public IRoadElement GetNextRandomElement( List<IRoadElement> route, Random rng )
        {
            return this._moveInformation.GetNextRandomElement( route, rng );
        }

        public void OnEnter( Car car )
        {
            this._carsInformation.Take( car );
        }

        public bool ShouldChange(Car car)
        {
            return this._moveInformation.ShouldChange( acutalCarLocation, car );
        }

        public float GetDistanceToStopLine()
        {
            return float.MaxValue;
        }

        public void GetCarAheadDistance( IRouteMark<IRoadElement> routMark, CarInformation carInformation )
        {
            this._carsInformation.GetCarAheadDistance( routMark, carInformation );
        }

        public void GetFirstCarToOutInformation( FirstCarToOutInformation carInformation )
        {
            this._carsInformation.GetFirstCarToOutInformation( carInformation );
        }

        public Vector2 GetCarDirection( Car car )
        {
            return this._moveInformation.GetCarDirection( car );
        }

        public void OnExit( Car car )
        {
            this._carsInformation.Remove( car );
        }

        public float GetCarDistanceToEnd( Car car )
        {
            return this._carsInformation.GetCarDistanceToEnd( car );
        }

        public bool IsPosibleToDriveFrom( IRoadElement roadElement )
        {
            return this._junctionInformation.IsPosibleToDriverFrom( roadElement );
        }

        public bool IsPosibleToDriveTo( IRoadElement roadElement )
        {
            return this._junctionInformation.IsPosibleToDriveTo( roadElement );
        }

        public float Lenght(IRoadElement previous, IRoadElement next)
        {
            return this._junctionInformation.Length(previous, next);
        }

        public bool CanStop(IRoadElement previous, IRoadElement next)
        {
            // BUG When turn left it can stay on junction
            return false;
        }
    }
}
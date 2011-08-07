using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Route;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors.LaneJunctionConductor
{
    public class RightHandRuleLaneJuctionConductor : IConductor
    {
        private readonly LaneJunction _laneJunction;

        private readonly LaneJunctionConductorCarsInformation _carsInformation;
        private readonly LaneJunctionConductorMoveInfomation _moveInformation;
        private readonly LaneJucntionConductorRightHandJunctionInformation _junctionInformation;
        private readonly LaneJunctionConductorLightInformation _lightInformation;

        public RightHandRuleLaneJuctionConductor( LaneJunction laneJunction )
        {
            Contract.Requires( laneJunction != null );
            this._laneJunction = laneJunction;
            this._carsInformation = new LaneJunctionConductorCarsInformation( this._laneJunction );
            this._moveInformation = new LaneJunctionConductorMoveInfomation( this._laneJunction );
            this._junctionInformation = new LaneJucntionConductorRightHandJunctionInformation( this._laneJunction );
            this._lightInformation = new LaneJunctionConductorLightInformation( this._laneJunction );
        }

        public IRoadElement GetNextRandomElement( List<IRoadElement> route, Random rng )
        {
            return this._moveInformation.GetNextRandomElement( route, rng );
        }

        public void Take( Car car )
        {
            this._carsInformation.Take( car );
        }

        public bool ShouldChange( Vector2 acutalCarLocation, Car car )
        {
            return this._moveInformation.ShouldChange( acutalCarLocation, car );
        }

        public float GetDistanceToStopLine()
        {
            return float.MaxValue;
        }

        public void GetLightInformation( IRouteMark routeMark, LightInfomration lightInformation )
        {
            this._lightInformation.GetLightInformation( routeMark, lightInformation );
        }

        public void GetNextJunctionInformation( IRouteMark route, JunctionInformation junctionInformation )
        {
            this._junctionInformation.GetNextJunctionInformation( route, junctionInformation );
        }

        public void GetCarAheadDistance( IRouteMark routMark, CarInformation carInformation )
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

        public void Remove( Car car )
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
    }
}
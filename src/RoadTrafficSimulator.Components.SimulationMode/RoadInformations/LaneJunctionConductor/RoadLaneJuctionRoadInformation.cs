using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.LaneJunctionConductor
{
    public class RoadLaneJuctionRoadInformation : IRoadInformation
    {
        private readonly LaneJunction _laneJunction;

        private readonly LaneJunctionConductorCarsInformation _carsInformation;
        private readonly LaneJunctionConductorMoveInfomation _moveInformation;
        private readonly LaneJucntionConductorRightHandJunctionInformation _junctionInformation;

        public RoadLaneJuctionRoadInformation( LaneJunction laneJunction )
        {
            Contract.Requires( laneJunction != null );
            this._laneJunction = laneJunction;
            this._carsInformation = new LaneJunctionConductorCarsInformation( this._laneJunction );
            this._moveInformation = new LaneJunctionConductorMoveInfomation( this._laneJunction );
            this._junctionInformation = new LaneJucntionConductorRightHandJunctionInformation( this._laneJunction );
        }

        public void OnEnter( Car car )
        {
            this._carsInformation.Take( car );
        }

        public void OnExit( Car car )
        {
            this._carsInformation.Remove( car );
        }

        public float Lenght( IRoadElement previous, IRoadElement next )
        {
            return Vector2.Distance( previous.BuildControl.Location, next.BuildControl.Location );
        }

        public bool ShouldChange( Car car )
        {
            return this._moveInformation.ShouldChange( car, car.Location );
        }

        public CarAhedInformation GetCarAheadDistance( Car car )
        {
            return this._carsInformation.GetCarAheadDistance( car ) ;
        }

        public void GetFirstCarToOutInformation( FirstCarToOutInformation carInformation )
        {
            throw new System.NotImplementedException();
        }

        public Vector2 GetCarDirection( Car car, IRoadElement nextPoint )
        {
            return this._junctionInformation.GetCarDirection( car, nextPoint );
        }

        public float GetCarDistanceTo( Car car, IRoadElement nextPoint )
        {
            throw new System.NotImplementedException();
        }

        public float GetDistanceToStopLine()
        {
            return float.MaxValue;
        }
    }
}
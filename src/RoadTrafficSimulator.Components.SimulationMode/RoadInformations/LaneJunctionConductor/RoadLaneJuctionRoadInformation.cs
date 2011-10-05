using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Route;

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

        public bool ShouldChange( Car car )
        {
//            return this._moveInformation.ShouldChange( car.Location, car );
            return false;
        }

        public float GetDistanceToStopLine()
        {
            return float.MaxValue;
        }
    }
}
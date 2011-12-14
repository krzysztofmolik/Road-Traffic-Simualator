using System;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.LaneJunctionConductor
{
    public class LaneJucntionConductorRightHandJunctionInformation
    {
        private readonly LaneJunction _laneJunction;
        public LaneJucntionConductorRightHandJunctionInformation( LaneJunction laneJunction )
        {
            this._laneJunction = laneJunction;
        }
        private IRoadElement GetLocationOfEdgeConnectedWith( IRoadElement roadElement )
        {
            if ( this._laneJunction.Bottom == roadElement ) { return this._laneJunction.Top; }
            if ( this._laneJunction.Top == roadElement ) { return this._laneJunction.Bottom; }
            if ( this._laneJunction.Right == roadElement ) { return this._laneJunction.Left; }
            if ( this._laneJunction.Left == roadElement ) { return this._laneJunction.Right; }
            throw new ArgumentException();
        }

        public float Length( IRoadElement previous, IRoadElement next )
        {
            var previousEdge = this.GetLocationOfEdgeConnectedWith( previous );
            var nextEdge = this.GetLocationOfEdgeConnectedWith( next );

            return Vector2.Distance( previousEdge.BuildControl.Location, nextEdge.BuildControl.Location );
        }

        public Vector2 GetCarDirection( Car car, IRoadElement nextPoint )
        {
            return nextPoint.BuildControl.Location - car.Location;
        }
    }
}
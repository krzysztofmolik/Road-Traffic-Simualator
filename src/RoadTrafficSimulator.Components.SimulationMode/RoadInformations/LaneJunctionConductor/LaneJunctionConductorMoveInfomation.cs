using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.LaneJunctionConductor
{
    public class LaneJunctionConductorMoveInfomation
    {
        private readonly LaneJunction _laneJunction;

        public LaneJunctionConductorMoveInfomation( LaneJunction laneJunction )
        {
            this._laneJunction = laneJunction;
        }

        public bool ShouldChange( Car car, Vector2 location )
        {
            var next = car.Conductors.GetNext().RoadElement;
            var distance = next.BuildControl.Location - location;
            if ( distance.Length() <= 0.001f ) { return true; }

            return Math.Sign( distance.X ) != Math.Sign( car.Direction.X ) && Math.Sign( distance.Y ) != Math.Sign( car.Direction.Y );
        }
    }
}
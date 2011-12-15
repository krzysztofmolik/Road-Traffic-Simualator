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
    }
}
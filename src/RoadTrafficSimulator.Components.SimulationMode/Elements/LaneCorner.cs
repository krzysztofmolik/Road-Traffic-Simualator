using System;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements
{
    public class LaneCorner : LaneConnector
    {
        public LaneCorner( RoadConnection control )
            : base( control )
        {
            this.LaneCornerBuild = control;
        }

        public RoadConnection LaneCornerBuild { get; private set; }

        public Lane Prev { get; set; }
        public Lane Next { get; set; }

        public override IConductor Condutor
        {
            get { return null; }
        }
    }
}
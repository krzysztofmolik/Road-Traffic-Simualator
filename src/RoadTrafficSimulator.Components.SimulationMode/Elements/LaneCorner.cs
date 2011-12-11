using System;
using System.Diagnostics.Contracts;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements
{
    public class LaneCorner : LaneConnector
    {
        public LaneCorner( RoadConnection control, Func<LaneCorner, IRoadInformation> conductorFactory )
            : base( control )
        {
            Contract.Requires( control != null ); Contract.Requires( conductorFactory != null ); Contract.Ensures( this.RoadInformation != null ); this.LaneCornerBuild = control;
            this.RoadInformation = conductorFactory( this );
        }

        public RoadConnection LaneCornerBuild { get; private set; }

        public Lane Prev { get; set; }
        public Lane Next { get; set; }

        public IRoadInformation RoadInformation { get; private set; }
    }
}
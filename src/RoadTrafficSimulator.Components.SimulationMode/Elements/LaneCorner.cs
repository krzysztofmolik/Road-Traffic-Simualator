using System;
using System.Diagnostics.Contracts;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements
{
    public class LaneCorner : LaneConnector
    {
        private readonly IConductor _conductor;

        public LaneCorner( RoadConnection control, Func<LaneCorner, IConductor> conductorFactory )
            : base( control )
        {
            Contract.Requires( control != null ); Contract.Requires( conductorFactory != null ); Contract.Ensures( this._conductor != null ); this.LaneCornerBuild = control;
            this._conductor = conductorFactory( this );
        }

        public RoadConnection LaneCornerBuild { get; private set; }

        public Lane Prev { get; set; }
        public Lane Next { get; set; }

        public override IConductor Condutor
        {
            get { return this._conductor; }
        }
    }
}
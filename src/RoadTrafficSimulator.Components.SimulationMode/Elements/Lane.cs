using System.Diagnostics.Contracts;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements
{
    public class Lane : RoadElementBase
    {
        private readonly IConductor _conductor;

        public Lane( RoadLaneBlock lane, IConductor conductor )
            : base( lane )
        {
            Contract.Requires( lane != null ); Contract.Requires( conductor != null );
            this.RoadLaneBlock = lane;
            this._conductor = conductor;
        }

        protected RoadLaneBlock RoadLaneBlock { get; set; }

        public LaneConnector Prev { get; set; }
        public LaneConnector Next { get; set; }
        public Lane Top { get; set; }
        public Lane Bottom { get; set; }

        public override IConductor Condutor
        {
            get { return this._conductor; }
        }
    }
}
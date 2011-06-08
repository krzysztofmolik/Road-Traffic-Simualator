using System;
using System.Diagnostics.Contracts;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements
{
    public class Lane : RoadElementBase
    {
        private readonly IConductor _conductor;

        public Lane( RoadLaneBlock lane, Func<Lane, IConductor> conductorFactory )
            : base( lane )
        {
            Contract.Requires( lane != null ); Contract.Requires( conductorFactory != null ); Contract.Ensures( this._conductor != null );
            this.RoadLaneBlock = lane;
            this._conductor = conductorFactory( this );
        }

        public RoadLaneBlock RoadLaneBlock { get; set; }

        public IRoadElement Prev { get; set; }
        public IRoadElement Next { get; set; }
        public Lane Top { get; set; }
        public Lane Bottom { get; set; }

        public override IConductor Condutor
        {
            get { return this._conductor; }
        }
    }
}
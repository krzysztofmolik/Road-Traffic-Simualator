using System;
using System.Diagnostics.Contracts;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements
{
    public class Lane : RoadElementBase
    {
        private readonly IRoadInformation _roadInformation;

        public Lane( RoadLaneBlock lane, Func<Lane, IRoadInformation> conductorFactory )
            : base( lane )
        {
            Contract.Requires( lane != null ); Contract.Requires( conductorFactory != null ); Contract.Ensures( this._roadInformation != null );
            this.RoadLaneBlock = lane;
            this._roadInformation = conductorFactory( this );
        }

        public RoadLaneBlock RoadLaneBlock { get; set; }

        public IRoadElement Prev { get; set; }
        public IRoadElement Next { get; set; }
        public Lane Top { get; set; }
        public Lane Bottom { get; set; }

        public override IRoadInformation RoadInformation
        {
            get { return this._roadInformation; }
        }
    }
}
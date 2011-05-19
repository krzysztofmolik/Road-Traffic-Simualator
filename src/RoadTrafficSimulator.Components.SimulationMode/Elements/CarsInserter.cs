using System;
using System.Diagnostics.Contracts;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements
{
    public class CarsInserter : LaneConnector
    {
        private readonly IConductor _conductor;
        public CarsInserter( BuildMode.Controls.CarsInserter control, Func<CarsInserter, IConductor> conductorFactory )
            : base( control )
        {
            Contract.Requires( control != null ); Contract.Requires( conductorFactory != null ); Contract.Ensures( this._conductor != null );
            this._conductor = conductorFactory( this );
            this.CarsInserterBuilder = control;
        }

        public Lane Lane { get; set; }
        public TimeSpan CarsInsertionInterval { get; set; }
        public DateTime LastTimeCarWasInseter { get; set; }

        public override IConductor Condutor
        {
            get { return this._conductor; }
        }

        public BuildMode.Controls.CarsInserter CarsInserterBuilder { get; set; }
    }
}
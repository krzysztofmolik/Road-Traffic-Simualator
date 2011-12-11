using System;
using System.Diagnostics.Contracts;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements
{
    public class CarsInserter : LaneConnector
    {
        public CarsInserter( BuildMode.Controls.CarsInserter control, Func<CarsInserter, IRoadInformation> conductorFactory )
            : base( control )
        {
            Contract.Requires( control != null ); Contract.Requires( conductorFactory != null ); Contract.Ensures( this.RoadInformation != null );
            this.RoadInformation = conductorFactory( this );
            this.CarsInserterBuilder = control;
            this.LastTimeCarWasInseter = DateTime.Now;
            this.CarsInsertionInterval = TimeSpan.FromMilliseconds( 500 );
        }

        public Lane Lane { get; set; }
        public TimeSpan CarsInsertionInterval { get; set; }
        public DateTime LastTimeCarWasInseter { get; set; }
        public IRoadInformation RoadInformation { get; private set; }

        public BuildMode.Controls.CarsInserter CarsInserterBuilder { get; set; }
    }
}
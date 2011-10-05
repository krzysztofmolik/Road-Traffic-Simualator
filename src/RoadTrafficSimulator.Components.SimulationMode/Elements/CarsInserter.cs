using System;
using System.Diagnostics.Contracts;
using RoadTrafficSimulator.Components.SimulationMode.Builder;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements
{
    public class CarsInserter : LaneConnector
    {
        private readonly IRoadInformation _roadInformation;
        public CarsInserter( BuildMode.Controls.CarsInserter control, Func<CarsInserter, IRoadInformation> conductorFactory )
            : base( control )
        {
            Contract.Requires( control != null ); Contract.Requires( conductorFactory != null ); Contract.Ensures( this._roadInformation != null );
            this._roadInformation = conductorFactory( this );
            this.CarsInserterBuilder = control;
            this.LastTimeCarWasInseter = DateTime.Now;
            this.CarsInsertionInterval = TimeSpan.FromMilliseconds( 500 );
        }

        public Lane Lane { get; set; }
        public TimeSpan CarsInsertionInterval { get; set; }
        public DateTime LastTimeCarWasInseter { get; set; }

        public override IRoadInformation RoadInformation
        {
            get { return this._roadInformation; }
        }

        public BuildMode.Controls.CarsInserter CarsInserterBuilder { get; set; }
    }
}
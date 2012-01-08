using System;
using System.Diagnostics.Contracts;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements
{
    public sealed class CarsInserter : LaneConnector
    {
        private readonly IRoadInformation _roadInformation;
        private readonly Random _rng = new Random();

        public CarsInserter( BuildMode.Controls.CarsInserter control, Func<CarsInserter, IRoadInformation> conductorFactory )
            : base( control )
        {
            Contract.Requires( control != null ); Contract.Requires( conductorFactory != null ); Contract.Ensures( this.Information != null );
            this._roadInformation = conductorFactory( this );
            this.CarsInserterBuilder = control;
            this.LastTimeCarWasInseter = DateTime.Now;
            this.CarsInsertionInterval = TimeSpan.FromMilliseconds( 500 );
        }

        public Lane Lane { get; set; }
        private TimeSpan _carsInsertionInterval;
        public TimeSpan CarsInsertionInterval
        {
            get { return this._carsInsertionInterval; }
            set 
            {
                this._carsInsertionInterval = value;
                var randomDelay = this._rng.Next( 0, (int) value.TotalMilliseconds );
                this.LastTimeCarWasInseter = DateTime.Now + TimeSpan.FromMilliseconds( randomDelay );
            }
        }

        public DateTime LastTimeCarWasInseter { get; set; }
        public BuildMode.Controls.CarsInserter CarsInserterBuilder { get; set; }

        public override IRoadInformation Information { get { return this._roadInformation; } }
    }
}
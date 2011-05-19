using System;
using System.Diagnostics.Contracts;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements
{
    public class CarsRemover : RoadElementBase
    {
        private readonly IConductor _conductor;

        public CarsRemover( BuildMode.Controls.CarsRemover control, Func<CarsRemover, IConductor> conductorFactory )
            : base( control )
        {
            Contract.Requires( control != null ); Contract.Requires( conductorFactory != null ); Contract.Ensures( this._conductor != null );
            this._conductor = conductorFactory( this );
            this.CarsRemoverBuilder = control;
        }

        public Lane Lane { get; set; }

        public override IConductor Condutor
        {
            get { return this._conductor; }
        }

        public BuildMode.Controls.CarsRemover CarsRemoverBuilder { get; set; }
    }
}
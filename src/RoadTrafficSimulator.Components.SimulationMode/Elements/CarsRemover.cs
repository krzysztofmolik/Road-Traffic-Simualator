using System;
using System.Diagnostics.Contracts;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements
{
    public class CarsRemover : RoadElementBase
    {
        public CarsRemover( BuildMode.Controls.CarsRemover control, Func<CarsRemover, IRoadInformation> conductorFactory )
            : base( control )
        {
            Contract.Requires( control != null ); Contract.Requires( conductorFactory != null ); Contract.Ensures( this.RoadInformation != null );
            this.RoadInformation = conductorFactory( this );
            this.CarsRemoverBuilder = control;
        }

        public Lane Lane { get; set; }

        public IRoadInformation RoadInformation { get; private set; }

        public BuildMode.Controls.CarsRemover CarsRemoverBuilder { get; set; }
    }
}
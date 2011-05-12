using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Common;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Messages;
using RoadTrafficSimulator.Infrastructure.Mouse;

namespace RoadTrafficSimulator.Components.BuildMode.Creators
{
    public class CarsRemoverCreator
    {
        private readonly IMouseInformation _mouseInformation;
        private bool _process;

        public CarsRemoverCreator( IMouseInformation mouseInformation, Factories.Factories factories, IEventAggregator eventAggregator )
        {
            Contract.Requires( mouseInformation != null );
            Contract.Requires( factories != null );
            Contract.Requires( eventAggregator != null );
            this._mouseInformation = mouseInformation;
            this._mouseInformation.LeftButtonPressed.Where( s => this.Process )
                .Subscribe( s =>
                                {
                                    var carInserter = new CarsRemover( factories, s.Location, null );
                                    eventAggregator.Publish( new AddControlToRoadLayer( carInserter ) );
                                } );
        }

        public bool Process
        {
            get { return this._process; }
        }

        public void Start()
        {
            if ( this._process ) { return; }
            this._process = true;
            this._mouseInformation.StartRecord();
        }
        public void Stop()
        {
            if ( this._process == false ) { return; }
            this._process = false;
            this._mouseInformation.StopRecord();
        }
    }
}
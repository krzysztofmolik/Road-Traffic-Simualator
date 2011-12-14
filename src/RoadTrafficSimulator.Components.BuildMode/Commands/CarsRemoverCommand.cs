using System;
using System.Diagnostics.Contracts;
using Common;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Messages;
using RoadTrafficSimulator.Infrastructure.Mouse;

namespace RoadTrafficSimulator.Components.BuildMode.Commands
{
    public class CarsRemoverCommand : ICommand
    {
        private readonly IMouseInformation _mouseInformation;
        private bool _process;

        public CarsRemoverCommand( IMouseInformation mouseInformation, Factories.Factories factories, IEventAggregator eventAggregator )
        {
            Contract.Requires( mouseInformation != null );
            Contract.Requires( factories != null );
            Contract.Requires( eventAggregator != null );
            this._mouseInformation = mouseInformation;
            this._mouseInformation.LeftButtonPressed.Subscribe( s =>
                                {
                                    var carInserter = new CarsRemover( factories, s.Location );
                                    eventAggregator.Publish( new AddControlToRoadLayer( carInserter ) );
                                } );
        }

        public CommandType CommandType
        {
            get { return CommandType.InsertCarsRemover; }
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
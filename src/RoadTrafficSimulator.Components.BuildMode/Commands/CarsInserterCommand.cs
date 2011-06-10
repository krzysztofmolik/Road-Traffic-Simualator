using System;
using System.Diagnostics.Contracts;
using Common;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Messages;
using RoadTrafficSimulator.Infrastructure.Mouse;

namespace RoadTrafficSimulator.Components.BuildMode.Commands
{
    public class CarsInserterCommand : ICommand
    {
        private readonly IMouseInformation _mouseInformation;

        public CarsInserterCommand( IMouseInformation mouseInformation, Factories.Factories factories, IEventAggregator eventAggregator )
        {
            Contract.Requires( mouseInformation != null );
            Contract.Requires( factories != null );
            Contract.Requires( eventAggregator != null );
            this._mouseInformation = mouseInformation;
            this._mouseInformation.LeftButtonClicked.Subscribe( s =>
                                                                {
                                                                    var carInserter = new CarsInserter( factories, s.Location, null );
                                                                    eventAggregator.Publish( new AddControlToRoadLayer( carInserter ) );
                                                                } );
        }

        public CommandType CommandType
        {
            get { return CommandType.InsertCarsInserter; }
        }

        public void Start()
        {
            this._mouseInformation.StartRecord();
        }

        public void Stop()
        {
            this._mouseInformation.StopRecord();
        }
    }
}
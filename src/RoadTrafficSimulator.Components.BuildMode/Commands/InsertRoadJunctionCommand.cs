using System;
using Common;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Mouse;

namespace RoadTrafficSimulator.Components.BuildMode.Commands
{
    public class InsertRoadJunctionCommand : ICommand
    {
        private readonly IMouseInformation _mouseInformation;
        private readonly Factories.Factories _factories;

        public InsertRoadJunctionCommand( IMouseInformation mouseInformation, Factories.Factories factories, IEventAggregator eventAggregator )
        {
            this._factories = factories;
            this._mouseInformation = mouseInformation.NotNull();

            this._mouseInformation.LeftButtonPressed.Subscribe( this.AddJunction );
        }

        private void AddJunction( XnaMouseState mouseState )
        {
            this._factories.ControlFactory.CreateRoadJunctioBlockWithEdges( mouseState.Location );
        }

        public CommandType CommandType
        {
            get { return CommandType.InsertRoadJunction; }
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
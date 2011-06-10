using System;
using Common;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Road;

namespace RoadTrafficSimulator.Components.BuildMode.Commands
{
    public class InsertRoadJunctionCommand : ICommand
    {
        private readonly ICompositeControl _owner;
        private readonly IMouseInformation _mouseInformation;
        private readonly Func<Vector2, ICompositeControl, IRoadJunctionBlock> _roadJunctionBlockFactory;

        public InsertRoadJunctionCommand( IMouseInformation mouseInformation, RoadLayer owner, Func<Vector2, ICompositeControl, IRoadJunctionBlock> roadJunctionBlockFactory)
        {
            this._owner = owner;
            this._roadJunctionBlockFactory = roadJunctionBlockFactory;
            this._mouseInformation = mouseInformation.NotNull();

            this._mouseInformation.LeftButtonPressed.Subscribe( this.AddJunction );
        }

        private void AddJunction( XnaMouseState mouseState )
        {
            var children = this._roadJunctionBlockFactory( mouseState.Location, this._owner );
            this._owner.AddChild( children );
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
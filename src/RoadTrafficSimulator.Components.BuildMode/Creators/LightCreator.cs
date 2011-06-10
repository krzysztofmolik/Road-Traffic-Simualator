using Common;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.DependencyInjection;
using RoadTrafficSimulator.Infrastructure.Messages;
using RoadTrafficSimulator.Infrastructure.Mouse;
using System;
using System.Linq;

namespace RoadTrafficSimulator.Components.BuildMode.Creators
{
    public class LightCommand : Command
    {
        private readonly RoadLayer _roadLayer;
        private readonly IMouseInformation _mouseInformation;
        private readonly IContentManager _contentManager;
        private readonly VisitAllChildren _visistator;

        public LightCommand(  RoadLayer roadLayer, IMouseInformation mouseInformation, IContentManager contentManager )
        {
            this._roadLayer = roadLayer;
            this._mouseInformation = mouseInformation;
            // This smells
            this._contentManager = contentManager;
            this._visistator = new VisitAllChildren( this._roadLayer );

            this._mouseInformation.LeftButtonClicked.Subscribe( this.MouseClicked );
        }

        private void MouseClicked( XnaMouseState mouseState )
        {
            var hittedControl = this._visistator.Where( s => s.IsHitted( mouseState.Location ) ).OfType<RoadJunctionEdge>().FirstOrDefault();
            if ( hittedControl == null ) { return; }
            if ( hittedControl.Connector.CanPutLights() )
            {
                var light = new Light( mouseState.Location, this._contentManager );
                // TODO this smells
                this._roadLayer.AddChild( light );

            }
        }

        public Type CreatedType
        {
            get { return typeof( Light ); }
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

    public interface ICommand
    {
        CommandType CommandType { get; }
        void Start();
        void Stop();
    }

    public enum CommandType
    {
        CreateCarsInserter,
        CreateCarsRemover,
        ConnectObject,
    }
}
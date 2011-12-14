using System;
using System.Linq;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.DependencyInjection;
using RoadTrafficSimulator.Infrastructure.Mouse;

namespace RoadTrafficSimulator.Components.BuildMode.Commands
{
    public class LightInsertCommand : ICommand
    {
        private readonly RoadLayer _roadLayer;
        private readonly IMouseInformation _mouseInformation;
        private readonly IContentManagerAdapter _contentManager;
        private readonly VisitAllChildren _visistator;

        public LightInsertCommand( RoadLayer roadLayer, IMouseInformation mouseInformation, IContentManagerAdapter contentManager )
        {
            this._roadLayer = roadLayer;
            this._mouseInformation = mouseInformation;
            // This smells
            this._contentManager = contentManager;
            this._visistator = new VisitAllChildren( this._roadLayer );

            this._mouseInformation.LeftButtonClicked.Subscribe( this.MouseClicked );
        }

        public CommandType CommandType
        {
            get { return CommandType.InserterLights; }
        }

        public void Start()
        {
            this._mouseInformation.StartRecord();
        }

        public void Stop()
        {
            this._mouseInformation.StopRecord();
        }

        private void MouseClicked( XnaMouseState mouseState )
        {
            var hittedControl = this._visistator.Where( s => s.IsHitted( mouseState.Location ) ).OfType<JunctionEdge>().FirstOrDefault();
            if ( hittedControl == null ) { return; }
            if ( hittedControl.Connector.CanPutLights() )
            {
                var light = new LightBlock( mouseState.Location, this._contentManager.Load( "lights" ) );
                light.Connector.ConnectWith( hittedControl );
                hittedControl.Connector.ConnectWithLight( light );
                // TODO this smells
                this._roadLayer.AddChild( light );

            }
        }
    }
}
using System;
using System.Diagnostics.Contracts;
using Common;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Mouse;
using System.Linq;

namespace RoadTrafficSimulator.Components.BuildMode.Commands
{
    public class SelectToEditCommand : ICommand
    {
        private readonly IMouseInformation _mouseInformation;
        private readonly VisitAllChildren _allControl;
        private readonly IEventAggregator _eventAggregator;

        public SelectToEditCommand( IMouseInformation mouseInformation, Factories.Factories factories, IEventAggregator eventAggregator, VisitAllChildren allControls )
        {
            Contract.Requires( mouseInformation != null );
            Contract.Requires( factories != null );
            Contract.Requires( eventAggregator != null );
            this._eventAggregator = eventAggregator;
            this._mouseInformation = mouseInformation;
            this._mouseInformation.LeftButtonClicked.Subscribe( this.OnLeftButtonClick );
            this._allControl = allControls;
        }

        private void OnLeftButtonClick( XnaMouseState xnaMouseState )
        {
            var clicedControl = this.FindControlAtPoint( xnaMouseState.Location );
            if ( clicedControl != null )
            {
                this._eventAggregator.Publish( new GuiCommdnEdit( clicedControl ) );
            }
        }

        private IControl FindControlAtPoint( Vector2 location )
        {
            return this._allControl.FirstOrDefault( c => c.IsHitted( location ) );
        }

        public CommandType CommandType
        {
            get { return CommandType.SelectToEdit; }
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
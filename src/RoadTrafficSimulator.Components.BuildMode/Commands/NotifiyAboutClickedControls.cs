using System;
using System.Diagnostics.Contracts;
using Common;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Mouse;
using System.Linq;

namespace RoadTrafficSimulator.Components.BuildMode.Commands
{
    public class NotifiyAboutClickedControls : ICommand
    {
        private readonly IMouseInformation _mouseInformation;
        private readonly VisitAllChildren _allControls;
        private readonly IEventAggregator _eventAggreagor;

        public NotifiyAboutClickedControls( IMouseInformation mouseInformation, IEventAggregator eventAggregator, VisitAllChildren allControls )
        {
            Contract.Requires( mouseInformation != null );
            Contract.Requires( eventAggregator != null );
            this._mouseInformation = mouseInformation;
            this._eventAggreagor = eventAggregator;
            this._allControls = allControls;
            this._mouseInformation.LeftButtonClicked.Subscribe( this.OnLeftButtonClicked );
        }

        public CommandType CommandType
        {
            get { return CommandType.NotifyAboutClickedControls; }
        }

        public void Start()
        {
            this._mouseInformation.StartRecord();
        }

        public void Stop()
        {
            this._mouseInformation.StopRecord();
        }

        private void OnLeftButtonClicked( XnaMouseState xnaMouseState )
        {
            var clickedControl = this._allControls.FirstOrDefault( s => s.IsHitted( xnaMouseState.Location ) );
            if ( clickedControl != null )
            {
                this._eventAggreagor.Publish( new GuiCommdnControlClicked( clickedControl ) );
            }
        }
    }
}
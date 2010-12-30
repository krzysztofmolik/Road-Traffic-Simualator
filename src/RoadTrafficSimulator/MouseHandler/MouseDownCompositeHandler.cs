using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Metadata;
using Common;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.MouseHandler.Infrastructure;
using XnaRoadTrafficConstructor.Infrastucure.Mouse;
using XnaVs10.Utils;

namespace XnaRoadTrafficConstructor.MouseHandler
{
    //TODO Remove it
    public class MouseDownCompositeHandler
    {
        private readonly IMouseInformation _mouseInformation;
        private readonly IEnumerable<IMouseHandler> _mouseDownHandlers;

        public MouseDownCompositeHandler(
            IMouseInformation mouseInformation,
            IEnumerable<Meta<IMouseHandler, IOrderMeta>> mouseDownHandlers )
        {
            this._mouseDownHandlers = mouseDownHandlers.OrderBy( m => m.Metadata.Order ).Select( m => m.Value ).ToArray();
            this._mouseInformation = mouseInformation.NotNull();
            this._mouseInformation.LeftButtonPressed.Subscribe( this.HandleMouseDown );
            this._mouseInformation.LeftButtonRelease.Subscribe( this.HandleMouseUp );
            this._mouseInformation.MousePositionChanged.Subscribe( this.HandleMouseMove );
            this._mouseInformation.LeftButtonClicked.Subscribe( this.HandleMouseClick );
            this._mouseInformation.StartRecord();
        }

        private void HandleMouseClick( XnaMouseState xnaMouseState )
        {
            this._mouseDownHandlers.FirstOrDefault( s => s.MouseClick( xnaMouseState ) );
        }

        private void HandleMouseMove( XnaMouseState xnaMouseState )
        {
            this._mouseDownHandlers.FirstOrDefault( s => s.MouseMove( xnaMouseState ) );
        }

        private void HandleMouseUp( XnaMouseState xnaMouseState )
        {
            this._mouseDownHandlers.FirstOrDefault( s => s.MouseUp( xnaMouseState ) );
        }

        private void HandleMouseDown( XnaMouseState xnaMouseState )
        {
            this._mouseDownHandlers.FirstOrDefault( s => s.MouseDown( xnaMouseState ) );
        }
    }
}
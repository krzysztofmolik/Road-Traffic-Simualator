using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework.Input;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.MouseHandler.Infrastructure;
using System.Linq;
using Common;

namespace RoadTrafficSimulator.Factories
{
    public class SelectControlCommand
    {
        private readonly List<ILogicControl> _selectedControls = new List<ILogicControl>();
        private readonly IMouseInformation _mouseInformation;
        private readonly KeyboardInputNotify _keyboard;
        private VisitAllChildren _allControls;
        private ILogicControl _lastSelectedControl = null;

        public SelectControlCommand( IMouseInformation mouseInformation, VisitAllChildren allControls, KeyboardInputNotify keyboard )
        {
            Contract.Requires( allControls != null );
            this._mouseInformation = mouseInformation;
            this._keyboard = keyboard;
            this._allControls = allControls;

            this.SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            this._mouseInformation.LeftButtonPressed.Subscribe( this.LeftButtonPressed );
            this._mouseInformation.LeftButtonRelease.Subscribe( this.LeftButtonReleased );
            this._mouseInformation.MousePositionChanged.Subscribe( this.MousePostionChanged );
        }

        private void LeftButtonPressed( XnaMouseState mouseState )
        {
            // TODO Refator this
            var selectedControl = this._allControls.Where( c => c.IsHitted( mouseState.Location ) ).FirstOrDefault();
            if ( selectedControl == null )
            {
                this._lastSelectedControl = null;
                this._selectedControls.ForEach( c => c.IsSelected = false );
                this._selectedControls.Clear();
                return;
            }

            var control = selectedControl.GetHittedControl( mouseState.Location );

            if ( this.MultiSelect() )
            {
                this._selectedControls.Add( control );
                control.IsSelected = true;
                this._lastSelectedControl = control;
            }
            else
            {
                this._selectedControls.ForEach( c => c.IsSelected = false );
                this._selectedControls.Clear();
                this._selectedControls.Add( control );
                control.IsSelected = true;
                this._lastSelectedControl = control;
            }
        }

        private void LeftButtonReleased( XnaMouseState mouseState )
        {
            this._lastSelectedControl = null;
        }

        private void MousePostionChanged( XnaMouseState mouseState )
        {
        }

        private bool MultiSelect()
        {
            var result = this._keyboard.IsKeyPressed( Keys.LeftControl );
            return result;
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
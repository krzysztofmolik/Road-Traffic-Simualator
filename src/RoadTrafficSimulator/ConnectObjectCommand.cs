using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.MouseHandler.Infrastructure;
using XnaRoadTrafficConstructor;

namespace RoadTrafficSimulator
{
    public class ConnectObjectCommand
    {
        private readonly IMouseInformation _mouseInformation;
        private readonly KeyboardInputNotify _keyboardInformation;
        private readonly CompositeConnectionCommand _compositeConnectionCommand;
        private readonly VisitAllChildren _visitator;

        private ILogicControl _lastClickedEdges;

        public ConnectObjectCommand(
                            IMouseInformation mouseInformation,
                            KeyboardInputNotify keyboardInformation,
                            CompositeConnectionCommand compositeConnectionCommand,
                            VisitAllChildren visitator )
        {
            this._mouseInformation = mouseInformation;
            this._keyboardInformation = keyboardInformation;
            this._compositeConnectionCommand = compositeConnectionCommand;
            this._visitator = visitator;
            this.SubscribeToMouseEvent();
        }

        public void End()
        {
            this._mouseInformation.StopRecord();
            this._lastClickedEdges = null;

        }

        public void Begin()
        {
            this._mouseInformation.StartRecord();
        }

        private void SubscribeToMouseEvent()
        {
            this._mouseInformation.LeftButtonClicked.Subscribe( this.LeftButtonClicked );
            this._keyboardInformation.KeyPressed.Where( s => s.Key == Keys.Escape )
                                                          .Subscribe( u => this.End() );
        }

        private void LeftButtonClicked( XnaMouseState mouseState )
        {
            var edges = this.FindControlAtPoint( mouseState.Location );
            if ( edges == null )
            {
                return;
            }

            if ( this._lastClickedEdges == null )
            {
                this._lastClickedEdges = edges;
                this._lastClickedEdges.IsSelected = true;
            }
            else
            {
                this.Begin( this._lastClickedEdges, edges );
                this._lastClickedEdges = null;
            }
        }

        private void Begin( ILogicControl first, ILogicControl second )
        {
            this._compositeConnectionCommand.Connect(first, second);
        }

        private ILogicControl FindControlAtPoint( Vector2 location )
        {
            // TODO Chenge it
            ILogicControl control = null;
            this._visitator.FirstOrDefault( s =>
                                                      {
                                                          var hited = s.GetHittedControl(location);
                                                          if (hited != null)
                                                          {
                                                              control = hited;
                                                              return true;
                                                          }
                                                          return false;
                                                      });
            return control;
        }
    }
}
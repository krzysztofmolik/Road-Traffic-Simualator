using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RoadTrafficSimulator.Components.BuildMode.Commands;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Mouse;

namespace RoadTrafficSimulator.Components.BuildMode
{
    public class ConnectObjectCommand : ICommand
    {
        private readonly IMouseInformation _mouseInformation;
        private readonly CompositeConnectionCommand _compositeConnectionCommand;
        private readonly VisitAllChildren _visitator;

        private ILogicControl _lastClickedEdges;

        public ConnectObjectCommand(
                            IMouseInformation mouseInformation,
                            CompositeConnectionCommand compositeConnectionCommand,
                            RoadLayer owner )
        {
            this._mouseInformation = mouseInformation;
            this._compositeConnectionCommand = compositeConnectionCommand;
            this._visitator = new VisitAllChildren( owner );

            this._mouseInformation.LeftButtonClicked.Subscribe( this.LeftButtonClicked );
        }

        public CommandType CommandType
        {
            get { return CommandType.ConnectObject; }
        }

        public void Stop()
        {
            this._mouseInformation.StopRecord();
            this._lastClickedEdges = null;
        }

        public void Start()
        {
            this._mouseInformation.StartRecord();
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
            this._compositeConnectionCommand.Connect( first, second );
        }

        private ILogicControl FindControlAtPoint( Vector2 location )
        {
            // TODO Chenge it
            ILogicControl control = null;
            this._visitator.FirstOrDefault( s =>
                                                      {
                                                          var hited = s.GetHittedControl( location );
                                                          if ( hited != null )
                                                          {
                                                              control = hited;
                                                              return true;
                                                          }
                                                          return false;
                                                      } );
            return control;
        }
    }
}
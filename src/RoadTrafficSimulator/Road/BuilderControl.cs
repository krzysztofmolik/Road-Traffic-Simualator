using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Integration;
using RoadTrafficSimulator.MouseHandler.Infrastructure;
using RoadTrafficSimulator.Utils;
using XnaRoadTrafficConstructor;
using Common;
using XnaVs10.Utils;

namespace RoadTrafficSimulator.Road
{
    public class ScreenZoom
    {
        private IMouseInformation _mouseInforamtion;
        private Camera3D _camera3D;

        public ScreenZoom( IMouseInformation mouseInformation, Camera3D camera3D )
        {
            this._mouseInforamtion = mouseInformation.NotNull();
            this._camera3D = camera3D.NotNull();

            this._mouseInforamtion.ScrollWheelChanged.Where( s => this.Zoom ).Subscribe( this.Zooming );
        }

        public bool Zoom { get; set; }

        private void Zooming( XnaMouseState mouseState )
        {
            this._camera3D.Zoom = mouseState.ScrollWheelValueDelta * 0.1f;
        }
    }
        
    public class BuilderControl
    {
        private readonly KeyboardInputNotify _keyboardInformation;
        private bool _connectingObject;
        private bool _addingRoadLane;
        private bool _addingRoadJunctionBlock;
        private MessageBroker _messageBroker;

        public BuilderControl( KeyboardInputNotify keyboardInformation )
        {
            this._keyboardInformation = keyboardInformation;

            this.SubscribeMessages();
        }

        public RoadComponent RoadComponent { get; set; }

        public bool AddingRoadLane
        {
            get { return this._addingRoadLane; }
            set
            {
                if ( this._addingRoadLane == value )
                {
                    return;
                }

                this._addingRoadLane = value;
                if ( this._addingRoadLane )
                {
                    this.RoadComponent.AddingRoadLaneBegin();
                }
                else
                {
                    this.RoadComponent.AddingRoadLaneEnd();
                }
            }
        }


        public bool ShowStopLine
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool ShowRoadDirection
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool AddingRoadJunctionBlock
        {
            get { return this._addingRoadJunctionBlock; }
            set
            {
                if ( this._addingRoadJunctionBlock == value )
                {
                    return;
                }

                this._addingRoadJunctionBlock = value;

                if ( this._addingRoadJunctionBlock )
                {
                    this.RoadComponent.AddingRoadJunctionBlockBegin();
                }
                else
                {
                    this.RoadComponent.AddingRoadJunctionBlockEnd();
                }
            }
        }

        private bool ConnectingObject
        {
            get { return this._connectingObject; }
            set
            {
                if ( this._connectingObject == value )
                {
                    return;
                }

                this._connectingObject = value;

                if ( this._connectingObject )
                {
                    this.RoadComponent.StartConnectingObject();
                }
                else
                {
                    this.RoadComponent.StopConnectingObject();
                }
            }
        }

        private void SubscribeMessages()
        {
            this._keyboardInformation.ObservableKeyPressed
                .Where( s => s.EventArgs.Key == Keys.C && this._keyboardInformation.IsKeyPressed( Keys.LeftAlt ) )
                .Subscribe( s => this.ConnectingObject = true );

            this._keyboardInformation.ObservableKeyRelease.Where( s => s.EventArgs.Key == Keys.Escape )
                .Subscribe( s => this.CancelAllOperation() );

            this._keyboardInformation.ObservableKeyPressed.Where( s => s.EventArgs.Key == Keys.A )
                .Subscribe( s => this.Zoom = true );
        }

        protected bool Zoom { get; set; }


        private void CancelAllOperation()
        {
            this.AddingRoadLane = false;
            this.AddingRoadJunctionBlock = false;
            this.ConnectingObject = false;
            this.Zoom = false;
        }
    }
}
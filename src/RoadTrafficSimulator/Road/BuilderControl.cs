using System;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace RoadTrafficSimulator.Road
{
    public class BuilderControl
    {
        private readonly KeyboardInputNotify _keyboardInformation;
        private bool _connectingObject;
        private bool _addingRoadLane;
        private bool _addingRoadJunctionBlock;
        private bool _selecteObject;

        public BuilderControl( KeyboardInputNotify keyboardInformation )
        {
            this._keyboardInformation = keyboardInformation;

            this.SubscribeMessages();
        }

        public RoadComponent RoadComponent { get; set; }

        public bool AddingRoadLane
        {
            get
            {
                return this._addingRoadLane;
            }

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
            get
            {
                return this._addingRoadJunctionBlock;
            }

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
            get
            {
                return this._connectingObject;
            }

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
            this._keyboardInformation.KeyPressed
                .Where( s => s.Key == Keys.C && this._keyboardInformation.IsKeyPressed( Keys.LeftAlt ) )
                .Subscribe( s => this.ConnectingObject = true );

            this._keyboardInformation.KeyPressed
                .Where( s => s.Key == Keys.S && this._keyboardInformation.IsKeyPressed( Keys.LeftAlt ) )
                .Subscribe( s => this.SelecteObject = true );

            this._keyboardInformation.KeyRelease.Where( s => s.Key == Keys.Escape )
                .Subscribe( s => this.CancelAllOperation() );
        }

        protected bool SelecteObject
        {
            get
            {
                return this._selecteObject;
            }

            set
            {
                if ( this._selecteObject == value )
                {
                    return;
                }

                this._selecteObject = value;

                if ( this._selecteObject )
                {
                    this.RoadComponent.StartSelectingObject();
                }
                else
                {
                    this.RoadComponent.StopSelectingObject();
                }
            }
        }

        private void CancelAllOperation()
        {
            this.AddingRoadLane = false;
            this.AddingRoadJunctionBlock = false;
            this.ConnectingObject = false;
        }
    }
}
using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Common.Extensions;
using Microsoft.Xna.Framework.Input;
using RoadTrafficSimulator.Components.BuildMode.Creators;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.BuildMode
{
    public class BuilderControl
    {
        private readonly KeyboardInputNotify _keyboardInformation;
        private readonly Lazy<CarsInserterCreator> _carsInserter;
        private readonly Lazy<CarsRemoverCreator> _carsRemover;
        private bool _connectingObject;
        private bool _addingRoadLane;
        private bool _addingRoadJunctionBlock;
        private bool _selecteObject;

        // TODO Check lazy
        public BuilderControl( KeyboardInputNotify keyboardInformation, Lazy<CarsInserterCreator> carsInserter, Lazy<CarsRemoverCreator> carsRemover )
        {
            Contract.Requires( keyboardInformation != null);
            Contract.Requires( carsInserter != null);
            this._keyboardInformation = keyboardInformation;
            this._carsInserter = carsInserter;
            this._carsRemover = carsRemover;

            this.SubscribeMessages();
        }

        public BuildModeMainComponent BuildModeMainComponent { get; set; }

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
                    this.BuildModeMainComponent.AddingRoadLaneBegin();
                }
                else
                {
                    this.BuildModeMainComponent.AddingRoadLaneEnd();
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
                    this.BuildModeMainComponent.AddingRoadJunctionBlockBegin();
                }
                else
                {
                    this.BuildModeMainComponent.AddingRoadJunctionBlockEnd();
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
                    this.BuildModeMainComponent.StartConnectingObject();
                }
                else
                {
                    this.BuildModeMainComponent.StopConnectingObject();
                }
            }
        }

        public void InsertCarsInserter()
        {
            this._carsInserter.Value.Start();
        }

        public void InsertCarsRemover()
        {
            this._carsRemover.Value.Start();
        }

        private void SubscribeMessages()
        {
            this._keyboardInformation.KeyPressed
                .Where( s => s.Key == Keys.C && this._keyboardInformation.IsKeyPressed( Keys.LeftAlt ) )
                .Subscribe( s => this.ConnectingObject = true );

            this._keyboardInformation.KeyRelease.Where( s => s.Key == Keys.Escape )
                .Subscribe( s => this.CancelAllOperation() );
        }

        private void CancelAllOperation()
        {
            this.AddingRoadLane = false;
            this.AddingRoadJunctionBlock = false;
            this.ConnectingObject = false;
            this._carsInserter.Value.Stop();
            this._carsRemover.Value.Stop();
        }
    }
}
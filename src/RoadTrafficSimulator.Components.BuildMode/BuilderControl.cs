using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Common;
using Microsoft.Xna.Framework.Input;
using RoadTrafficSimulator.Components.BuildMode.Creators;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Messages;

namespace RoadTrafficSimulator.Components.BuildMode
{
    public class BuilderControl : IHandle<InsertControl>
    {
        private readonly KeyboardInputNotify _keyboardInformation;
        private readonly IEnumerable<ICreator> _creators;
        private readonly Lazy<CarsInserterCreator> _carsInserter;
        private readonly Lazy<CarsRemoverCreator> _carsRemover;
        private readonly IEventAggregator _eventAggregator;
        private bool _connectingObject;
        private bool _addingRoadLane;
        private bool _addingRoadJunctionBlock;
        private bool _selecteObject;

        // TODO Check lazy
        public BuilderControl( KeyboardInputNotify keyboardInformation, IEnumerable<ICreator> creators, IEventAggregator eventAggregator )
        {
            Contract.Ensures( this._keyboardInformation != null );
            Contract.Ensures( this._carsInserter != null );
            Contract.Ensures( this._eventAggregator != null );
            this._keyboardInformation = keyboardInformation;
            this._creators = creators.ToArray();
            this._eventAggregator = eventAggregator;
            this._eventAggregator.Subscribe( this );

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

        public void AddLights()
        {
            this._eventAggregator.Publish( new InsertLights() );
        }

        public void Handle( InsertControl message )
        {
            this._creators.ForEach( c => c.Stop() );
        }
    }
}
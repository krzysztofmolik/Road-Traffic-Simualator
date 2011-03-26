using System;
using System.Collections.Generic;
using Autofac;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Integration;
using RoadTrafficSimulator.Road.Controls;
using RoadTrafficSimulator.RoadTrafficSimulatorMessages;
using DrawableGameComponent = RoadTrafficSimulator.Infrastructure.DrawableGameComponent;

namespace RoadTrafficSimulator.Road
{
    // TODO Change name
    public class RoadComponent : DrawableGameComponent, IDisposable
    {
        private readonly RoadLaneCreatorController _roadLaneCreator;
        private readonly IEnumerable<IBackgroundJob> _backgroundJobs;
        private readonly RoadJunctionCreator _roadJunctionCreator;
        private readonly IMouseInformation _mouseInformation;
        private readonly ConnectObjectCommand _connectObjectCommand;
        private readonly IEventAggregator _eventAggreator;
        private readonly Func<Vector2, ICompositeControl, IRoadJunctionBlock> _roadJunctionBlockFactory;
        private RoadLayer _roadLayer;

        public RoadComponent(
                    IEnumerable<IBackgroundJob> backgroundJobs,
                    MessageBroker messageBroker,
                    RoadLaneCreatorController roadLaneCreator,
                    RoadJunctionCreator roadJunctionCreator,
                    IMouseInformation mouseInformation,
                    ConnectObjectCommand connectObjectCommand,
                    IEventAggregator eventAggreator,
                    Func<Vector2, ICompositeControl, IRoadJunctionBlock> roadJunctionBlockFactory,
                    IGraphicsDeviceService graphicsDeviceService, RoadLayer roadLayer )
            : base( graphicsDeviceService )
        {
            this._backgroundJobs = backgroundJobs;
            this._roadJunctionCreator = roadJunctionCreator;
            this._connectObjectCommand = connectObjectCommand;
            this._eventAggreator = eventAggreator;
            this._roadJunctionBlockFactory = roadJunctionBlockFactory;
            this._mouseInformation = mouseInformation;
            this._roadLaneCreator = roadLaneCreator;
            this.MessageBroker = messageBroker.NotNull();
            this._roadLayer = roadLayer;

            this.SubscribeToMessages();
            this.SubscribeToMouseInformation();

            this._mouseInformation.StartRecord();

            this._backgroundJobs.ForEach( s => s.Start() );
        }

        private void SubscribeToMouseInformation()
        {
            this._mouseInformation.LeftButtonPressed.Subscribe( s => this._roadLayer.MouseSupport.OnLeftButtonPressed( s ) );
            this._mouseInformation.LeftButtonRelease.Subscribe( s => this._roadLayer.MouseSupport.OnLeftButtonReleased( s ) );
            this._mouseInformation.LeftButtonClicked.Subscribe( s => this._roadLayer.MouseSupport.OnLeftButtonClick( s ) );
            this._mouseInformation.MousePositionChanged.Subscribe( s => this._roadLayer.MouseSupport.OnMove( s ) );
        }

        private void SubscribeToMessages()
        {
            this._roadJunctionCreator.JunctionCreated.Subscribe( location =>
                                                                    {
                                                                        var children = this._roadJunctionBlockFactory( location, this._roadLayer );
                                                                        this._roadLayer.AddChild( children );
                                                                    } );
        }

        public void StartConnectingObject()
        {
            this._connectObjectCommand.Begin();
        }

        public void StopConnectingObject()
        {
            this._connectObjectCommand.End();
        }

        protected MessageBroker MessageBroker { get; set; }

        public void AddLight( Unit unit )
        {
            throw new NotImplementedException();

            //            this._roadLayer.DrawPossibleLightLocation = true; 
            //            var setInto =
            //                from pressed in this._controlManager.MousePressedObservable
            //                let mousePosition = this._camera.ToSpace( pressed.EventArgs.MousePosition )
            //                let light = this._roadLayer.GetLightPosition( mousePosition )
            //                where light != null
            //                select light;
            //
            //            var keyEscape = this._controlManager.KeyPressedObservable.Where( t =>
            //                                                                            t.EventArgs.Key == Keys.Escape
            //                );
            //            setInto.TakeUntil( keyEscape ).Subscribe(
            //                t => this._roadLayer.SetLight( t ),
            //                () => this._roadLayer.DrawPossibleLightLocation = false );
        }

        protected override void UnloadContent()
        {
        }

        protected override void LoadContent()
        {
            this._roadLaneCreator.SetOwner( this._roadLayer );

            this._eventAggreator.Publish( new XnaWindowInitialized() );
        }

        public override void Draw( GameTime gameTime )
        {
            this._roadLayer.Draw( gameTime );
            base.Draw( gameTime );
        }

        public void Dispose()
        {
            this._backgroundJobs.ForEach( s => s.Stop() );
        }

        public void AddingRoadJunctionBlockBegin()
        {
            this._roadJunctionCreator.Begin();
        }

        public void AddingRoadJunctionBlockEnd()
        {
            this._roadJunctionCreator.End();
        }

        public void AddingRoadLaneBegin()
        {
            this._roadLaneCreator.Begin( this._roadLayer );
        }

        public void AddingRoadLaneEnd()
        {
            this._roadLaneCreator.End();
        }
    }
}
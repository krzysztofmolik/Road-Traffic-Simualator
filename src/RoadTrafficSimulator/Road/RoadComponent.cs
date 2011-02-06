using System;
using System.Collections.Generic;
using Autofac;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Factories;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Integration;
using RoadTrafficSimulator.Road.Controls;
using RoadTrafficSimulator.RoadTrafficSimulatorMessages;
using WinFormsGraphicsDevice;

namespace RoadTrafficSimulator.Road
{
    // TODO Change name
    public class RoadComponent : XnaComponent, IDisposable
    {
        private readonly RoadLaneCreatorController _roadLaneCreator;
        private readonly IEnumerable<IBackgroundJob> _backgroundJobs;
        private readonly RoadJunctionCreator _roadJunctionCreator;
        private readonly IMouseInformation _mouseInformation;
        private readonly ConnectObjectCommand _connectObjectCommand;
        private readonly IEventAggregator _eventAggreator;
        private readonly Func<Vector2, ICompositeControl, IRoadJunctionBlock> _roadJunctionBlockFactory;
        private readonly SelectControlCommand _selectCommand;
        private RoadLayer _roadLayer;

        public RoadComponent(
                    IEnumerable<IBackgroundJob> backgroundJobs,
                    GraphicsDevice graphicsDevice,
                    ContentManager content,
                    MessageBroker messageBroker,
                    RoadLaneCreatorController roadLaneCreator,
                    RoadJunctionCreator roadJunctionCreator,
                    IMouseInformation mouseInformation,
                    ConnectObjectCommand connectObjectCommand,
                    IEventAggregator eventAggreator,
                    Func<Vector2, ICompositeControl, IRoadJunctionBlock> roadJunctionBlockFactory,
                    SelectControlCommand selectCommand)
            : base( content, graphicsDevice )
        {
            this._backgroundJobs = backgroundJobs;
            this._selectCommand = selectCommand;
            this._roadJunctionCreator = roadJunctionCreator;
            this._connectObjectCommand = connectObjectCommand;
            this._eventAggreator = eventAggreator;
            this._roadJunctionBlockFactory = roadJunctionBlockFactory;
            this._mouseInformation = mouseInformation;
            this._roadLaneCreator = roadLaneCreator;
            this.MessageBroker = messageBroker.NotNull();

            this.Initialize();
        }

        private void Initialize()
        {
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
                this._roadLayer.AddChild( this._roadJunctionBlockFactory( location, this._roadLayer ) ) );
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

        public override void LoadContent( IContainer serviceLocator )
        {
            this._roadLayer = serviceLocator.Resolve<RoadLayer>();
            this._roadLaneCreator.SetOwner( this._roadLayer );

            this._eventAggreator.Publish( new XnaWindowInitialized() );
        }

        public override void Draw( TimeSpan time )
        {
            this._roadLayer.Draw( time );
            base.Draw( time );
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

        public void StopSelectingObject()
        {
            this._selectCommand.Stop();
        }

        public void StartSelectingObject()
        {
            this._selectCommand.Start();
        }
    }
}
using System;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.BuildMode.Creators;
using RoadTrafficSimulator.Infrastructure.Messages;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Road;
using DrawableGameComponent = RoadTrafficSimulator.Infrastructure.DrawableGameComponent;

namespace RoadTrafficSimulator.Components.BuildMode
{
    public class BuildModeMainComponent : DrawableGameComponent, IHandle<AddControlToRoadLayer>, IDisposable
    {
        private readonly RoadLaneCreatorController _roadLaneCreator;
        private readonly RoadJunctionCreator _roadJunctionCreator;
        private readonly IMouseInformation _mouseInformation;
        private readonly ConnectObjectCommand _connectObjectCommand;
        private readonly IEventAggregator _eventAggreator;
        private readonly Func<Vector2, ICompositeControl, IRoadJunctionBlock> _roadJunctionBlockFactory;
        private RoadLayer _roadLayer;

        public BuildModeMainComponent(
                    RoadLaneCreatorController roadLaneCreator,
                    RoadJunctionCreator roadJunctionCreator,
                    IMouseInformation mouseInformation,
                    ConnectObjectCommand connectObjectCommand,
                    IEventAggregator eventAggreator,
                    Func<Vector2, ICompositeControl, IRoadJunctionBlock> roadJunctionBlockFactory,
                    IGraphicsDeviceService graphicsDeviceService, RoadLayer roadLayer )
            : base( graphicsDeviceService )
        {
            this._roadJunctionCreator = roadJunctionCreator;
            this._connectObjectCommand = connectObjectCommand;
            this._eventAggreator = eventAggreator;
            this._roadJunctionBlockFactory = roadJunctionBlockFactory;
            this._mouseInformation = mouseInformation;
            this._roadLaneCreator = roadLaneCreator;
            this._roadLayer = roadLayer;

            this.SubscribeToMessages();
            this.SubscribeToMouseInformation();

            this._mouseInformation.StartRecord();
        }

        private void SubscribeToMouseInformation()
        {
            this._mouseInformation.LeftButtonPressed.Subscribe( s => this._roadLayer.MouseHandler.OnLeftButtonPressed( s ) );
            this._mouseInformation.LeftButtonRelease.Subscribe( s => this._roadLayer.MouseHandler.OnLeftButtonReleased( s ) );
            this._mouseInformation.LeftButtonClicked.Subscribe( s => this._roadLayer.MouseHandler.OnLeftButtonClick( s ) );
            this._mouseInformation.MousePositionChanged.Subscribe( s => this._roadLayer.MouseHandler.OnMove( s ) );
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
        }

        public override void Draw( GameTime gameTime )
        {
            this._roadLayer.Draw( gameTime );
            base.Draw( gameTime );
        }

        public void Dispose()
        {
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

        public void Handle( AddControlToRoadLayer message )
        {
            var control = message.Control;
            control.Parent = this._roadLayer;
            this._roadLayer.AddChild( control );
        }
    }
}
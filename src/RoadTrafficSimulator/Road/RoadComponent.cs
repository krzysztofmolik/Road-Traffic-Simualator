using System;
using Autofac;
using Common;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Integration;
using RoadTrafficSimulator.Road.RoadJoiners;
using WinFormsGraphicsDevice;
using Xna;
using XnaRoadTrafficConstructor.Infrastucure.Mouse;
using XnaRoadTrafficConstructor.Road;
using XnaVs10.Road;

namespace RoadTrafficSimulator.Road
{
    //TODO Change name

    public class RoadComponent : XnaComponent, IDisposable
    {
        private RoadLayer _roadLayer;
        private readonly IControlManager _controlManager;
        private LineDrawer2D _lineDrawer2D;
        private readonly RoadLaneCreator _roadLaneCreator;
        private readonly RoadJunctionCreator _roadJunctionCreator;
        private readonly IMouseInformation _mouseInformation;
        private readonly ConnectObjectCommand _connectObjectCommand;

        public RoadComponent(
                    GraphicsDevice graphicsDevice,
                    IControlManager controlManager,
                    ContentManager content,
                    MessageBroker messageBroker,
                    RoadLaneCreator roadLaneCreator,
                    RoadJunctionCreator roadJunctionCreator,
                    IMouseInformation mouseInformation,
                    ConnectObjectCommand connectObjectCommand )
            : base( content, graphicsDevice )
        {
            this._roadJunctionCreator = roadJunctionCreator;
            this._connectObjectCommand = connectObjectCommand;
            this._mouseInformation = mouseInformation;
            this._roadLaneCreator = roadLaneCreator;
            this._controlManager = controlManager;
            this.MessageBroker = messageBroker.NotNull();
            this._controlManager.KeyReleased += this.OnKeyRelease;

            this.SubscribeToMessages();
            this.SubscribeToMouseInformation();

            this._mouseInformation.StartRecord();
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
            this._roadLaneCreator.RoadCreated.Subscribe( roadLane => this._roadLayer.AddChild( roadLane ) );
            this._roadJunctionCreator.JunctionCreated.Subscribe( location =>
                this._roadLayer.AddChild( new RoadJunctionBlock( location, this._roadLayer ) ) );

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

        public void OnKeyRelease( object sender, KeyboardKeysChangedArgs e )
        {
            if ( e.Key == Keys.Escape )
            {
                this._lineDrawer2D.IsEnabled = false;
            }
        }

        public void AddLight( Unit unit )
        {
            throw new NotImplementedException();
            //            this._roadLayer.DrawPossibleLightLocation = true;
            //
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
            this._lineDrawer2D = serviceLocator.Resolve<LineDrawer2D>();
        }

        public override void Draw( TimeSpan time )
        {
            this._roadLayer.Draw( time );
            base.Draw( time );
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
    }
}
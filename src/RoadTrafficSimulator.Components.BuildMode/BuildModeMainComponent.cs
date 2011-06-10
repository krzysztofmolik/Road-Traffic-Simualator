using System;
using System.Collections;
using System.Collections.Generic;
using Arcane.Xna.Presentation;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.BuildMode.Creators;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Messages;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Road;
using DrawableGameComponent = RoadTrafficSimulator.Infrastructure.DrawableGameComponent;
using System.Linq;

namespace RoadTrafficSimulator.Components.BuildMode
{
    // TODO Is IHandle<AddControlToRoadLayer> needed ??
    public class BuildModeMainComponent : DrawableGameComponent, IHandle<AddControlToRoadLayer>, IUnitializable
    {
        private readonly RoadLaneCommandController _roadLaneCommand;
        private readonly RoadJunctionCreator _roadJunctionCreator;
        private readonly IMouseInformation _mouseInformation;
        private readonly ConnectObjectCommand _connectObjectCommand;
        private readonly Func<Vector2, ICompositeControl, IRoadJunctionBlock> _roadJunctionBlockFactory;
        private readonly RoadLayer _roadLayer;
        private readonly IEnumerable<Command> _creators;
        private readonly List<IDisposable> _subscribtions = new List<IDisposable>();
        private IEventAggregator _eventAggreator;

        public BuildModeMainComponent(
                    RoadLaneCommandController roadLaneCommand,
                    RoadJunctionCreator roadJunctionCreator,
                    IMouseInformation mouseInformation,
                    ConnectObjectCommand connectObjectCommand,
                    IEventAggregator eventAggreator,
                    Func<Vector2, ICompositeControl, IRoadJunctionBlock> roadJunctionBlockFactory,
                    IGraphicsDeviceService graphicsDeviceService, 
                    RoadLayer roadLayer,
                    IEnumerable<Command> creators )
            : base( graphicsDeviceService, eventAggreator )
        {
            this._eventAggreator = eventAggreator;
            this._eventAggreator.Subscribe( this );
            this._roadJunctionCreator = roadJunctionCreator;
            this._connectObjectCommand = connectObjectCommand;
            this._roadJunctionBlockFactory = roadJunctionBlockFactory;
            this._mouseInformation = mouseInformation;
            this._roadLaneCommand = roadLaneCommand;
            this._roadLayer = roadLayer;
            this._creators = creators;

            this.Subscribe();

            this._mouseInformation.StartRecord();
        }

        public IEnumerable<IControl> GetAllBuildControls()
        {
            return this._roadLayer.Children.ToArray();
        }

        private void Subscribe()
        {
            this._subscribtions.Add( this._mouseInformation.LeftButtonPressed.Subscribe( s => this._roadLayer.MouseHandler.OnLeftButtonPressed( s ) ) );
            this._subscribtions.Add( this._mouseInformation.LeftButtonRelease.Subscribe( s => this._roadLayer.MouseHandler.OnLeftButtonReleased( s ) ) );
            this._subscribtions.Add( this._mouseInformation.LeftButtonClicked.Subscribe( s => this._roadLayer.MouseHandler.OnLeftButtonClick( s ) ) );
            this._subscribtions.Add( this._mouseInformation.MousePositionChanged.Subscribe( s => this._roadLayer.MouseHandler.OnMove( s ) ) );
            this._subscribtions.Add( this._mouseInformation.DoubleClick.Subscribe( s => this.ChangedCenterPoint( s.Location ) ) );
            this._subscribtions.Add( this._roadJunctionCreator.JunctionCreated.Subscribe( location =>
                                                                    {
                                                                        var children = this._roadJunctionBlockFactory( location, this._roadLayer );
                                                                        this._roadLayer.AddChild( children );
                                                                    } ) );
        }

        private void ChangedCenterPoint( Vector2 location )
        {
            this._eventAggreator.Publish( new CenterPointChanged( location ) );
        }

        private void Unsubscribe()
        {
            this._subscribtions.ForEach( s => s.Dispose() );
            this._subscribtions.Clear();
        }

        public void StartConnectingObject()
        {
            this._connectObjectCommand.Begin();
        }

        public void StopConnectingObject()
        {
            this._connectObjectCommand.End();
        }

        public override void Initialize()
        {
            base.Initialize();
            this.Subscribe();
            this._mouseInformation.StartRecord();
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
            Console.WriteLine( "UnloadContent" );
            base.UnloadContent();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this._roadLaneCommand.SetOwner( this._roadLayer );
        }

        public override void Draw( GameTime time )
        {
            this._roadLayer.Draw( time );
            base.Draw( time );
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
            this._roadLaneCommand.Begin( this._roadLayer );
        }

        public void AddingRoadLaneEnd()
        {
            this._roadLaneCommand.End();
        }

        public void Handle( AddControlToRoadLayer message )
        {
            var control = message.Control;
            //            control.Parent = this._roadLayer;
            this._roadLayer.AddChild( control );
        }

        public void Unitialize()
        {
            this.Unsubscribe();
            this._mouseInformation.StopRecord();
        }
    }
}
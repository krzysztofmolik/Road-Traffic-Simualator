using System;
using System.Collections.Generic;
using System.IO;
using Arcane.Xna.Presentation;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.BuildMode.Messages;
using RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Messages;
using RoadTrafficSimulator.Infrastructure.Mouse;
using DrawableGameComponent = RoadTrafficSimulator.Infrastructure.DrawableGameComponent;
using System.Linq;

namespace RoadTrafficSimulator.Components.BuildMode
{
    // TODO Is IHandle<AddControlToRoadLayer> needed ??
    public class BuildModeMainComponent : DrawableGameComponent, IHandle<AddControlToRoadLayer>, IHandle<SaveMap>, IHandle<LoadMap>, IUnitializable
    {
        private readonly IMouseInformation _mouseInformation;
        private readonly RoadLayer _roadLayer;
        private readonly List<IDisposable> _subscribtions = new List<IDisposable>();
        private readonly IEventAggregator _eventAggreator;
        private readonly BuilderCommandManager _commandManager;
        private ControlSerializer _serializer;

        public BuildModeMainComponent(
                    IMouseInformation mouseInformation,
                    IEventAggregator eventAggreator,
                    IGraphicsDeviceService graphicsDeviceService,
                    RoadLayer roadLayer,
                    BuilderCommandManager commandManager,
                    ControlSerializer serializer )
            : base( graphicsDeviceService, eventAggreator )
        {
            this._eventAggreator = eventAggreator;
            this._eventAggreator.Subscribe( this );
            this._mouseInformation = mouseInformation;
            this._roadLayer = roadLayer;
            this._commandManager = commandManager;
            this._serializer = serializer;

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

        public override void Initialize()
        {
            base.Initialize();
            this.Subscribe();
            this._mouseInformation.StartRecord();
        }

        public override void Draw( GameTime time )
        {
            this._roadLayer.Draw( time );
            base.Draw( time );
        }

        public void Handle( AddControlToRoadLayer message )
        {
            var control = message.Control;
            this._roadLayer.AddChild( control );
        }

        public void Unitialize()
        {
            this.Unsubscribe();
            this._mouseInformation.StopRecord();
        }

        public void Handle( SaveMap message )
        {
            using ( message.Stream )
            {
                this._serializer.Save( message.Stream, this._roadLayer.Children );
            }
        }

        public void Handle( LoadMap message )
        {
            using ( message.Stream )
            {
                var controls = this._serializer.Load( message.Stream );
                controls.ForEach( this._roadLayer.AddChild );
            }
        }
    }
}
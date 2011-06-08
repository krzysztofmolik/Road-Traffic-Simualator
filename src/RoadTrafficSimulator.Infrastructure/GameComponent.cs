using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Common;
using RoadTrafficSimulator.Infrastructure.Messages;

namespace RoadTrafficSimulator.Infrastructure
{
    public abstract class GameComponent : IGameComponent
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IGraphicsDeviceService _graphicsDeviceService;

        protected GameComponent( IGraphicsDeviceService graphicsDeviceService, IEventAggregator eventAggregator )
        {
            this._eventAggregator = eventAggregator;
            this._graphicsDeviceService = graphicsDeviceService.NotNull();
            this._graphicsDeviceService.DeviceCreated += this.DeviceCreated;
            this._graphicsDeviceService.DeviceDisposing += this.DeviceDisposing;
        }

        private void DeviceDisposing( object sender, EventArgs e )
        {
            this.UnloadContent();
        }

        protected virtual void UnloadContent()
        {
            this._eventAggregator.Publish( new UnloadConntent( this.GetType() ) );
        }

        private void DeviceCreated( object sender, EventArgs e )
        {
            this.LoadContent();
        }

        protected virtual void LoadContent()
        {
            this._eventAggregator.Publish( new IntializeContent( this.GetType() ) );
        }

        public virtual void Initialize()
        {
        }
    }
}
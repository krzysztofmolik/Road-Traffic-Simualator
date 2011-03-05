using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Common;

namespace RoadTrafficSimulator.Infrastructure
{
    public abstract class GameComponent : IGameComponent
    {
        private readonly IGraphicsDeviceService _graphicsDeviceService;

        protected GameComponent( IGraphicsDeviceService graphicsDeviceService )
        {
            this._graphicsDeviceService = graphicsDeviceService.NotNull();
            this._graphicsDeviceService.DeviceCreated += this.DeviceCreated;
            this._graphicsDeviceService.DeviceDisposing += this.DeviceDisposing;
        }

        private void DeviceDisposing( object sender, EventArgs e )
        {
            this.UnloadContent();
        }

        protected abstract void UnloadContent();

        private void DeviceCreated( object sender, EventArgs e )
        {
            this.LoadContent();
        }

        protected abstract void LoadContent();

        public virtual void Initialize()
        {
        }
    }
}
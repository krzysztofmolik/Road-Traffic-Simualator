using System;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RoadTrafficSimulator.Infrastructure
{
    public abstract class UpdateableGameComponent : GameComponent, IUpdateable
    {
        private bool _enabled;

        protected UpdateableGameComponent(IGraphicsDeviceService graphicsDeviceService, IEventAggregator eventAggregator) 
            : base( graphicsDeviceService, eventAggregator )
        {
            this.UpdateOrder = 1;
            this.Enabled = true;
        }

        private int _updateOrder;

        public virtual void Update( GameTime gameTime )
        {
        }

        public bool Enabled
        {
            get { return this._enabled; }
            private set
            {
                if ( this._enabled == value )
                {
                    return;
                }

                this._enabled = value;
                this.InvokeEnabledChanged();
            }
        }

        public int UpdateOrder
        {
            get { return this._updateOrder; }
            private set
            {
                this._updateOrder = value;
                this.InvokeUpdateOrderChanged();
            }
        }

        public event EventHandler<EventArgs> EnabledChanged;

        public void InvokeEnabledChanged()
        {
            var handler = EnabledChanged;
            if ( handler != null ) handler( this, EventArgs.Empty );
        }

        public event EventHandler<EventArgs> UpdateOrderChanged;

        public void InvokeUpdateOrderChanged()
        {
            var handler = UpdateOrderChanged;
            if ( handler != null ) handler( this, EventArgs.Empty );
        }
    }
}
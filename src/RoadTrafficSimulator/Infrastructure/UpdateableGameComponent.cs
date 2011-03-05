using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RoadTrafficSimulator.Infrastructure
{
    public abstract class UpdateableGameComponent : GameComponent, IUpdateable
    {
        private bool _enabled;

        protected UpdateableGameComponent(IGraphicsDeviceService graphicsDeviceService) 
            : base( graphicsDeviceService )
        {
            this.UpdateOrder = 1;
            this.Enabled = true;
        }

        private int _updateOrder;

        public void Update( GameTime gameTime )
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

    public abstract class DrawableGameComponent : UpdateableGameComponent , IDrawable
    {
        private bool _visible;
        private int _drawOrder;

        protected DrawableGameComponent(IGraphicsDeviceService graphicsDeviceService) 
            : base(graphicsDeviceService)
        {
            this.Visible = true;
            this.DrawOrder = 1;
        }

        public virtual void Draw(GameTime gameTime)
        {
        }

        public bool Visible
        {
            get { return this._visible; }
            set
            {
                if ( this._visible == value )
                {
                    return;
                }
                this._visible = value;
                this.InvokeVisibleChanged();
            }
        }

        public int DrawOrder
        {
            get { return this._drawOrder; }
            set
            {
                if ( this._drawOrder == value )
                {
                    return;
                }

                this._drawOrder = value;
                this.InvokeDrawOrderChanged();
            }

        }

        public event EventHandler<EventArgs> VisibleChanged;

        public void InvokeVisibleChanged()
        {
            var handler = VisibleChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public event EventHandler<EventArgs> DrawOrderChanged;

        public void InvokeDrawOrderChanged()
        {
            var handler = DrawOrderChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}
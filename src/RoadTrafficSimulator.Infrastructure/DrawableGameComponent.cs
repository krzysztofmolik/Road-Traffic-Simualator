using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RoadTrafficSimulator.Infrastructure
{
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

        public virtual void Draw(GameTime time)
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
using System;
using Autofac;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WinFormsGraphicsDevice
{
    public abstract class XnaComponent
    {
        protected XnaComponent( ContentManager contentManager, GraphicsDevice graphicsDevice )
        {
            this.Content = contentManager;
            this.GraphicsDevice = graphicsDevice;
        }

        public ContentManager Content { get; private set; }

        public int DrawOrder { get; set; }

        protected GraphicsDevice GraphicsDevice { get; set; }

        public virtual void Draw( TimeSpan time )
        {
        }

        public virtual void Update( TimeSpan time )
        {
        }

        public virtual void LoadContent( IContainer serviceLocator )
        {
        }

        public virtual void UnloadContent()
        {
        }

        public virtual void Refresh()
        {
        }
    }
}
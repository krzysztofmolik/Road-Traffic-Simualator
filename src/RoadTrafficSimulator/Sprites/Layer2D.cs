using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Xna;
using DrawableGameComponent = RoadTrafficSimulator.Infrastructure.DrawableGameComponent;

namespace XnaVs10.Sprites
{
    public class Layer2D : DrawableGameComponent
    {
        private readonly PrimitiveBatch _primitiveBatch;

        private List<ICustomDrawable> _items;

        public Layer2D( ContentManager content, GraphicsDevice graphicsDevice, PrimitiveBatch primitiveBatch, IGraphicsDeviceService graphicsDeviceService) 
            : base( graphicsDeviceService )
        {
            this._items = new List<ICustomDrawable>();
            this._primitiveBatch = primitiveBatch;
        }

        public void Add(ICustomDrawable item)
        {
            this._items.Add( item );
        }

        public void Remove(ICustomDrawable item)
        {
            this._items.Remove( item );
        }

        public override void Draw( GameTime gameTime )
        {
//            this._items.ForEach( i => i.Draw( gameTime, this._primitiveBatch ) );

            base.Draw( gameTime );
        }

        public void UpdateGraphicDevice()
        {
            this._primitiveBatch.UpdateBasicEffect();
        }

        protected override void UnloadContent()
        {
        }

        protected override void LoadContent()
        {
        }
    }
}
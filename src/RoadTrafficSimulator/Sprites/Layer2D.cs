using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WinFormsGraphicsDevice;
using Xna;

namespace XnaVs10.Sprites
{
    public class Layer2D : XnaComponent
    {
        private readonly PrimitiveBatch _primitiveBatch;

        private List<ICustomDrawable> _items;

        public Layer2D( ContentManager content, GraphicsDevice graphicsDevice, PrimitiveBatch primitiveBatch ) 
            : base( content, graphicsDevice )
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

        public override void Draw( TimeSpan gameTime )
        {
            this._items.ForEach( i => i.Draw( gameTime, this._primitiveBatch ) );

            base.Draw( gameTime );
        }

        public void UpdateGraphicDevice()
        {
            this._primitiveBatch.UpdateBasicEffect();
        }
    }
}
using System;
using System.Collections.Generic;
using Common;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Road;
using RoadTrafficSimulator.Utils;
using Xna;
using XnaVs10.Utils;

namespace XnaRoadTrafficConstructor.Road
{
    public class VertexPositionTextureDrawer
    {
        private readonly Camera3D _camera;
        private readonly DrawingHelper _drawerHelper;
        private readonly BasicEffect _basicEffect;
        private readonly Queue<Action> _actionBuffer = new Queue<Action>();

        public VertexPositionTextureDrawer( GraphicsDevice graphicDevice, Camera3D camera3D )
        {
            this._camera = camera3D.NotNull();
            this._camera.Changed += this.UpdateBasicEffect;

            this._basicEffect = new BasicEffect( graphicDevice )
                                    {
                                        Projection = this._camera.Projection,
                                        World = this._camera.World,
                                        View = this._camera.View,
                                        TextureEnabled = true,
                                    };

            this._drawerHelper = new DrawingHelper( this._basicEffect, graphicDevice );
        }

        private void UpdateBasicEffect(object sender, CameraChangedEventArgs e)
        {
            this._basicEffect.Projection = this._camera.Projection;
            this._basicEffect.View = this._camera.View;
            this._basicEffect.World = this._camera.World;
        }

        public void Draw( VertexPositionTexture[] block )
        {
            this._drawerHelper.DrawTriangeList( block );
        }

        public void Flush()
        {
            using ( this._drawerHelper.UnitOfWork )
            {
                this._actionBuffer.ForEach( a => a() );
            }
        }

        public void DrawTriangeList( Texture2D texture, VertexPositionTexture[] block )
        {
            this._actionBuffer.Enqueue( () =>
                                           {
                                               try
                                               {
                                                   this._basicEffect.Texture = texture;
                                                   this._drawerHelper.DrawTriangeList( block );
                                               }
                                               finally
                                               {
                                                   this._basicEffect.Texture = null;
                                               }
                                           } );
        }
    }
}
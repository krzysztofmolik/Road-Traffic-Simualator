using System;
using System.Collections.Generic;
using Common;
using Common.Xna;
using Common.Xna.FSharp;
using Microsoft.Xna.Framework.Graphics;

namespace RoadTrafficSimulator.Infrastructure.Draw
{
    public class VertexPositionTextureDrawer
    {
        private readonly Camera3D _camera;
        private BasicEffect _basicEffect;
        private readonly Queue<Action> _actionBuffer = new Queue<Action>();
        private readonly IGraphicsDeviceService _graphicsDeviceService;

        public VertexPositionTextureDrawer( Camera3D camera3D, IGraphicsDeviceService graphicsDeviceService )
        {
            this._graphicsDeviceService = graphicsDeviceService;
            this._camera = camera3D.NotNull();
            this._camera.Changed += this.UpdateBasicEffect;

            this._graphicsDeviceService.DeviceCreated += this.RecreateBassicEffect;
            this.CreateBasicEffect();
        }

        private void RecreateBassicEffect( object sender, EventArgs e )
        {
            this.CreateBasicEffect();
        }

        private void CreateBasicEffect()
        {
            this._basicEffect = new BasicEffect( this._graphicsDeviceService.GraphicsDevice )
                                    {
                                        Projection = this._camera.Projection,
                                        World = this._camera.World,
                                        View = this._camera.View,
                                        TextureEnabled = true,
                                        
                                    };
        }

        private void UpdateBasicEffect( object sender, CameraChangedEventArgs e )
        {
            this._basicEffect.Projection = this._camera.Projection;
            this._basicEffect.View = this._camera.View;
            this._basicEffect.World = this._camera.World;
        }

        public void Flush()
        {
            this._basicEffect.Begin();
            this._actionBuffer.ForEach( a => a() );
            this._actionBuffer.Clear();
        }

        public void DrawTriangeList( Texture2D texture, VertexPositionTexture[] block )
        {
            this._actionBuffer.Enqueue( () =>
                                           {
                                               try
                                               {
                                                   this._basicEffect.Texture = texture;
                                                   this._graphicsDeviceService.GraphicsDevice.DrawTriangleList( block );
                                               }
                                               finally
                                               {
                                                   this._basicEffect.Texture = null;
                                               }
                                           } );
        }

        public void DrawIndexedTraingeList( Texture2D texture, VertexPositionTexture[] block, short[] indexes )
        {
            this._actionBuffer.Enqueue( () =>
                                           {
                                               try
                                               {
                                                   if ( this._basicEffect.Texture != texture )
                                                   {
                                                       this._basicEffect.Texture = texture;
                                                       this._basicEffect.Begin();
                                                   }

                                                   this._graphicsDeviceService.GraphicsDevice.DrawIndexedUserPrimitives( block, indexes );
                                               }
                                               finally
                                               {
                                                   this._basicEffect.Texture = null;
                                               }
                                           } );
        }
    }
}
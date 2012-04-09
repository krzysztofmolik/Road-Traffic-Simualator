using System;
using System.Collections.Generic;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Common.Xna;
using Common.Xna.FSharp;

namespace RoadTrafficSimulator.Infrastructure.Draw
{
    public class VertexPositionColorDrawer
    {
        private readonly Camera3D _camera;
        private BasicEffect _basicEffect;

        private readonly Queue<Action> _actionBuffer = new Queue<Action>();
        private readonly IGraphicsDeviceService _graphicsDeviceService;

        public VertexPositionColorDrawer( IGraphicsDeviceService deviceService, Camera3D camera3D )
        {
            this._camera = camera3D.NotNull();
            this._camera.Changed += this.UpdateBasicEffect;
            this._graphicsDeviceService = deviceService;
            this._graphicsDeviceService.DeviceCreated += this.RecreateBassicEffect;
            this.CreateBasicEffect();

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
                                        VertexColorEnabled = true,
                                        DiffuseColor = Vector3.One,
                                        TextureEnabled = false,
                                    };
        }

        public void DrawTriangeList( VertexPositionColor[] block )
        {
            this._actionBuffer.Enqueue( () => this._graphicsDeviceService.GraphicsDevice.DrawTriangleList( block ) );
        }

        public void DrawIndexedTriangeList( VertexPositionColor[] block, short[] indexes )
        {
            this._actionBuffer.Enqueue( () => this._graphicsDeviceService.GraphicsDevice.DrawIndexedUserPrimitives( block, indexes ) );
        }

        public void Flush()
        {
            this._basicEffect.Begin();
            this._actionBuffer.ForEach( a => a() );
            this._actionBuffer.Clear();
        }

        private void UpdateBasicEffect( object sedner, EventArgs args )
        {
            this._basicEffect.Projection = this._camera.Projection;
            this._basicEffect.View = this._camera.View;
            this._basicEffect.World = this._camera.World;
        }
    }
}
using System;
using System.Collections.Generic;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Road;
using RoadTrafficSimulator.Utils;
using XnaVs10.Utils;

namespace RoadTrafficSimulator.Infrastructure.Draw
{
    public class VertexPositionColorDrawer
    {
        private readonly Camera3D _camera;
        private readonly DrawingHelper _drawerHelper;
        private readonly BasicEffect _basicEffect;

        private readonly Queue<Action> _actionBuffer = new Queue<Action>();
        public VertexPositionColorDrawer( GraphicsDevice graphicDevice, Camera3D camera3D )
        {
            this._camera = camera3D.NotNull();

            this._basicEffect = new BasicEffect(graphicDevice)
                                    {
                                        Projection = this._camera.Projection,
                                        World = this._camera.World,
                                        View = this._camera.View,
                                        VertexColorEnabled = true,
                                        DiffuseColor = Vector3.One,
                                        TextureEnabled = false,
                                    };

            this._drawerHelper = new DrawingHelper( this._basicEffect, graphicDevice );
        }

        public void DrawTriangeList( VertexPositionColor[] block )
        {
            this._actionBuffer.Enqueue( () => this._drawerHelper.DrawTriangeList( block ) );
        }

        public void Flush()
        {
            using ( this._drawerHelper.UnitOfWork )
            {
                this._actionBuffer.ForEach(a => a());
                this._actionBuffer.Clear();
            }
        }
    }
}
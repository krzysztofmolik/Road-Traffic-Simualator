using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xna;
using Xna.Extension;
using XnaVs10;
using XnaVs10.Extension;
using Game = Arcane.Xna.Presentation.Game;

namespace RoadTrafficSimulator.Utils
{
    public class Camera3D : IDisposable
    {
        private readonly Arcane.Xna.Presentation.Game _game;
        private float _zoom;

        public Camera3D( Game game )
        {
            this._game = game;
            this._zoom = MathHelper.PiOver2;
            this.InitCamera();
            this.UpdateCamera();
        }

        public event EventHandler<CameraChangedEventArgs> Changed;

        public Matrix View { get; private set; }

        public Matrix World { get; private set; }

        public Matrix Projection { get; private set; }

        public float Zoom
        {
            get { return this._zoom; }
            set
            {
                if ( this._zoom == value )
                {
                    return;
                }

                this._zoom = value;

                this.Projection = this.CreateProjection( this._zoom );
                this.UpdateCamera();
            }
        }


        protected float AspectRatio { get; private set; }

        public Vector2 ToSpace( Vector2 point )
        {
            var tranlate = this._game.GraphicsDevice.Viewport.Unproject(
                point.ToVector3(),
                this.Projection,
                this.View,
                this.World );

            return ( tranlate + new Vector3( 0, 0, -29 ) ).ToVector2();
        }

        public void Dispose()
        {
//            this._game.DeviceReset -= this.OnDeviceReset;
        }

        private void InitCamera()
        {
            this.AspectRatio = this._game.GraphicsDevice.Viewport.AspectRatio;
            this.View = Matrix.CreateLookAt( new Vector3( 0, 0, 1 ), new Vector3( 0, 0, 0 ), Vector3.Up );
            this.World = Matrix.Identity;
            this.Projection = this.CreateProjection( this.Zoom );
        }

        private Matrix CreateProjection( float zoom )
        {
            return Matrix.CreatePerspectiveFieldOfView( zoom, this.AspectRatio, 1, 500 );
        }

        private void OnDeviceReset( object sender, EventArgs e )
        {
            this.UpdateCamera();
        }

        private void UpdateCamera()
        {
            this.Changed.Raise( this, new CameraChangedEventArgs() );
        }
    }
}
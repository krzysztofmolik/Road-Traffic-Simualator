using System;
using System.Diagnostics.Contracts;
using Common;
using Common.Extensions;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Extension;
using RoadTrafficSimulator.Infrastructure.Messages;
using Xna;
using Game = Arcane.Xna.Presentation.Game;

namespace RoadTrafficSimulator.Infrastructure
{
    public class Camera3D : IHandle<ChangedZoom>, IDisposable
    {
        private readonly Game _game;
        private float _zoom;

        public Camera3D( Game game, IEventAggregator eventAggregator )
        {
            Contract.Requires( game != null );
            Contract.Requires( eventAggregator != null );

            this._game = game;
            this._zoom = MathHelper.PiOver2;
            eventAggregator.Subscribe( this );
            this.InitCamera();
            this.UpdateCamera();
            this._game.Window.SizeChanged += ( sender, args ) =>
                                                 {
                                                     this.AspectRatio = this._game.GraphicsDevice.Viewport.AspectRatio;
                                                     this.Projection = this.CreateProjection( this._zoom );
                                                     this.UpdateCamera();
                                                 };
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
                var newValue = value;
                if ( newValue < 0.01f ) { newValue = 0.01f; }
                if ( newValue > MathHelper.Pi - 0.01f ) { newValue = MathHelper.Pi - 0.01f; }
                this._zoom = newValue;
                this.Projection = this.CreateProjection( this._zoom );
                this.UpdateCamera();
            }
        }


        protected float AspectRatio { get; private set; }

        public Vector2 ToSpace( Vector2 point )
        {
            // NOTE Ugly workaround about dispose problem
            if ( this._game.GraphicsDevice == null ) { return point; }

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
            if ( zoom > MathHelper.Pi ) { zoom = ( float ) Math.Round( MathHelper.Pi, 2 ); }
            if ( zoom < 0 ) { zoom = 0.01f; }

            return Matrix.CreatePerspectiveFieldOfView( zoom, this.AspectRatio, 1, 500 );
        }

        private void UpdateCamera()
        {
            this.Changed.Raise( this, new CameraChangedEventArgs() );
        }

        public void Handle( ChangedZoom message )
        {
            var zoomPercentValue = MathHelper.Pi * message.Percent * 0.01f;

            this.Zoom = MathHelper.Pi - zoomPercentValue;
        }
    }
}
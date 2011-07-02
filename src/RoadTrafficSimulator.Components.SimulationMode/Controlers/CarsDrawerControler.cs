using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Messages;
using RoadTrafficSimulator.Infrastructure.DependencyInjection;
using RoadTrafficSimulator.Infrastructure.Draw;
using RoadTrafficSimulator.Infrastructure.Extension;
using RoadTrafficSimulator.Infrastructure.Messages;
using System.Linq;

namespace RoadTrafficSimulator.Components.SimulationMode.Controlers
{
    public class CarsDrawerControlerItem
    {
        private Vector2 _currentDirection;
        private Car _car;
        private Vector3[] _orginalCarBounds;
        private readonly Vector2[] _texturePosition;
        private readonly int[] _indexes;
        private Vector3[] _currentCarBound;

        public CarsDrawerControlerItem( Car car )
        {
            this._car = car;

            this._orginalCarBounds = new Vector3[ 4 ];
            this._orginalCarBounds[ 0 ] = new Vector3( -this._car.Lenght / 2, -this._car.Width / 2, 0.0f );
            this._orginalCarBounds[ 1 ] = new Vector3( this._car.Lenght / 2, -this._car.Width / 2, 0.0f );
            this._orginalCarBounds[ 2 ] = new Vector3( this._car.Lenght / 2, this._car.Width / 2, 0.0f );
            this._orginalCarBounds[ 3 ] = new Vector3( -this._car.Lenght / 2, this._car.Width / 2, 0.0f );
            this._currentCarBound = this._orginalCarBounds.ToArray();

            this._texturePosition = new Vector2[ 4 ];
            this._texturePosition[ 0 ] = new Vector2( 0f, 0f );
            this._texturePosition[ 1 ] = new Vector2( 1f, 0f );
            this._texturePosition[ 2 ] = new Vector2( 1f, 1f );
            this._texturePosition[ 3 ] = new Vector2( 0f, 1f );

            this._indexes = new[] { 0, 3, 1, 1, 3, 2 };
        }

        public Car Car { get { return this._car; } }

        public void Draw( Graphic graphic, Texture2D texture )
        {
            if ( this._currentDirection != this.Car.Direction )
            {
                this._currentDirection = this.Car.Direction;
                this.UpdateRotateMatix( this.Car.Direction );
            }

            var vertex = this._currentCarBound.Select( s => s + this.Car.Location.ToVector3() ).Select( ( v, i ) => new VertexPositionTexture( v, this._texturePosition[ i ] ) ).ToArray();
            graphic.VertexPositionalTextureDrawer.DrawIndexedTraingeList( texture, vertex, this._indexes );
        }

        private void UpdateRotateMatix( Vector2 direction )
        {
            var angel = direction.Angel();
            var rotationMatrix = Matrix.CreateRotationZ( angel );
            this._currentCarBound = this._orginalCarBounds.Select( s => Vector3.Transform( s, rotationMatrix ) ).ToArray();
        }
    }

    public class CarsDrawerControler : IControlers, IHandle<CarCreated>, IHandle<CarRemoved>, IHandle<UnloadConntent>, IHandle<IntializeContent>
    {
        private readonly object _contentLock = new object();
        private readonly List<CarsDrawerControlerItem> _cars = new List<CarsDrawerControlerItem>();
        private readonly IEventAggregator _eventAggregator;
        private readonly IContentManager _contentManager;
        private readonly Graphic _graphic;
        private Texture2D _carTexture;

        public CarsDrawerControler( IEventAggregator eventAggregator, IContentManager contentManager, Graphic graphic )
        {
            Contract.Requires( eventAggregator != null ); Contract.Requires( contentManager != null ); Contract.Requires( graphic != null );
            this._eventAggregator = eventAggregator;
            this._contentManager = contentManager;
            this._graphic = graphic;
            this._eventAggregator.Subscribe( this );
        }

        public void AddControl( IRoadElement element ) { }

        public void Draw( GameTime gameTime )
        {
            this._cars.ForEach( c => c.Draw( this._graphic, this._carTexture ) );
            this._graphic.VertexPositionalTextureDrawer.Flush();
            this._graphic.VertexPositionalColorDrawer.Flush();
        }

        public void Update( GameTime gameTime ) { }

        public int Order
        {
            get { return ( int ) Infrastructure.Order.High; }
        }

        public void Handle( CarCreated message )
        {
            Contract.Requires( message != null );
            lock ( this._contentLock )
            {
                this._cars.Add( new CarsDrawerControlerItem( message.Car ) );
            }
        }

        public void Handle( CarRemoved message )
        {
            Contract.Requires( message != null );
            this._cars.RemoveAll( s => s.Car == message.Car );
        }

        public void Handle( UnloadConntent message )
        {
            if ( message.ComponentType != typeof( SimulationModeMainComponent ) ) { return; }
            lock ( this._contentLock )
            {
                if ( this._carTexture != null )
                {
                    this._carTexture.Dispose();
                }
                this._carTexture = null;
            }
        }

        public void Handle( IntializeContent message )
        {
            if ( message.ComponentType != typeof( SimulationModeMainComponent ) ) { return; }
            lock ( this._contentLock )
            {
                if ( this._carTexture != null ) { return; }
                this._carTexture = this._contentManager.Load<Texture2D>( "PassengerCar" );
            }
        }
    }
}
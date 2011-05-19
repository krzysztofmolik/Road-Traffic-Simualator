using System;
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

namespace RoadTrafficSimulator.Components.SimulationMode.Controlers
{
    public class CarsDrawerControler : IControlers, IHandle<CarCreated>, IHandle<CarRemoved>
    {
        private readonly List<Car> _cars = new List<Car>();
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

            this._carTexture = contentManager.Load<Texture2D>( "PassengerCar" );
        }

        public void AddControl( IRoadElement element ) { }

        public void Draw( GameTime gameTime )
        {
            this._cars.ForEach( this.Draw );
            this._graphic.VertexPositionalTextureDrawer.Flush();
            this._graphic.VertexPositionalColorDrawer.Flush();
        }

        private void Draw( Car car )
        {
            var translationVector = car.Location - new Vector2( car.Width / 2, car.Lenght / 2 );
            var upperLeft = new Vector2( 0.0f, 0.0f ) + translationVector;
            var upperRight = new Vector2( 0.0f, car.Width ) + translationVector;
            var lowerRight = new Vector2( car.Width, car.Lenght ) + translationVector;
            var lowerLeft = new Vector2( 0.0f, car.Lenght ) + translationVector;
            this._graphic.VertexPositionalTextureDrawer.DrawIndexedTraingeList(
                this._carTexture,
                new[]
                    {
                        new VertexPositionTexture(upperLeft.ToVector3(), new Vector2(0f, 0f)),
                        new VertexPositionTexture(upperRight.ToVector3(), new Vector2(1f, 0f)),
                        new VertexPositionTexture(lowerRight.ToVector3(), new Vector2(1f, 1f)),
                        new VertexPositionTexture(lowerLeft.ToVector3(), new Vector2(0f, 1f)),
                    },
                new[] { 3, 0, 1, 3, 1, 2 } );
        }

        public void Update( GameTime gameTime )
        {
            throw new NotImplementedException();
        }

        public void Handle( CarCreated message )
        {
            Contract.Requires( message != null );
            this._cars.Add( message.Car );
        }

        public void Handle( CarRemoved message )
        {
            Contract.Requires( message != null );
            this._cars.Remove( message.Car );
        }
    }
}
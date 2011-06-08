using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Common;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Messages;

namespace RoadTrafficSimulator.Components.SimulationMode.Controlers
{
    public class CarUpdateControler : IControlers, IHandle<CarCreated>, IHandle<CarRemoved>
    {
        private readonly object _lock = new object();
        private readonly List<Car> _cars = new List<Car>();

        public CarUpdateControler( IEventAggregator eventAggregator )
        {
            Contract.Requires( eventAggregator != null );
            eventAggregator.Subscribe( this );
        }

        public void AddControl( IRoadElement element ) { }

        public void Draw( GameTime gameTime ) { }

        public void Update( GameTime gameTime )
        {
            // TODO CHange it
            lock ( this._lock )
            {
                this._cars.ForEach( c => c.StateMachine.Update( gameTime.ElapsedGameTime ) );
            }
        }

        public int Order
        {
            get { return (int) SimulationMode.Order.Normal; }
        }

        public void Handle( CarCreated message )
        {
            lock ( this._lock )
            {
                this._cars.Add( message.Car );
            }
        }

        public void Handle( CarRemoved message )
        {
            lock ( this._lock )
            {
                this._cars.Remove( message.Car );
            }
        }
    }
}
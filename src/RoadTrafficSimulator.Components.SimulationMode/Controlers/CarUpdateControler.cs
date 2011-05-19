using System;
using System.Collections.Generic;
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

        public void AddControl( IRoadElement element ) { }

        public void Draw( GameTime gameTime ) { }

        public void Update( GameTime gameTime )
        {
            // TODO CHange it
            var timeFrame = TimeSpan.FromMilliseconds( 33 );
            lock ( this._lock )
            {
                this._cars.ForEach( c => c.StateMachine.Update( timeFrame ) );
            }
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
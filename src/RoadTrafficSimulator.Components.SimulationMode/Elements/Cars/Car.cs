using System;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements.Cars
{
    public class Car
    {
        private readonly Route.Route _route = new Route.Route();
        private readonly CarStateMachine _stateMachine;

        public Car()
        {
            this._stateMachine = new CarStateMachine( this );
        }

        public int CarId { get; set; }
        private float _velocity;
        public float Velocity
        {
            get { return _velocity; }
            set
            {
                if( this._velocity - value > UnitConverter.FromKmPerHour( 15  ) )
                {
                    Console.WriteLine( "Error" );
                }
                _velocity = value;
            }
        }

        public float MaxSpeed { get; set; }
        public Vector2 Location { get; set; }
        public Vector2 Direction { get; set; }
        public float Lenght { get; set; }
        public float Width { get; set; }
        public float BreakingForce { get; set; }
        public float AccelerateForce { get; set; }
        public Route.Route Route { get { return this._route; } }
        public CarStateMachine StateMachine { get { return this._stateMachine; } }
    }

}
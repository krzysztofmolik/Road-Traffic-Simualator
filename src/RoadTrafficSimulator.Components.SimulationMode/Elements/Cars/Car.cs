using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using RoadTrafficSimulator.Infrastructure;
using CarStateMachine = RoadTrafficSimulator.Components.SimulationMode.RoadInformations.CarStateMachine;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements.Cars
{
    public class Car
    {
        private readonly CarStateMachine _stateMachine;
        private readonly IRouteMark<IConductor> _conductors;
        private float _velocity;

        public Car( IEnumerable<IConductor> route )
        {
            this._conductors = new RouteMark<IConductor>( new Route<IConductor>( route ) );
            this._stateMachine = new CarStateMachine( this );
        }

        public int CarId { get; set; }

        public float Velocity
        {
            get { return _velocity; }
            set
            {
                if ( this._velocity - value > UnitConverter.FromKmPerHour( 15 ) )
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
        public CarStateMachine StateMachine { get { return this._stateMachine; } }
        public IRouteMark<IConductor> Conductors { get { return this._conductors; } }
    }
}

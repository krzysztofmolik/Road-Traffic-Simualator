using System;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations.New;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using RoadTrafficSimulator.Infrastructure;
using CarStateMachine = RoadTrafficSimulator.Components.SimulationMode.Conductors.CarStateMachine;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements.Cars
{
    public class Car
    {
        private readonly Route.Route<IRoadElement> _roadElements = new Route.Route<IRoadElement>();
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
        public Route.Route<IRoadElement> RoadElements { get { return this._roadElements; } }
        public CarStateMachine StateMachine { get { return this._stateMachine; } }

        public IRouteMark<IConductor> Conductors
        {
            get { throw new NotImplementedException(); }
        }
    }
}

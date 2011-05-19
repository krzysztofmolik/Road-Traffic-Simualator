using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements.Cars
{
    public class Car
    {
        private readonly Queue<IRoadElement> _route = new Queue<IRoadElement>();
        private readonly CarStateMachine _stateMachine;

        public Car()
        {
            this._stateMachine = new CarStateMachine( this );
        }

        public float Velocity { get; set; }
        public Vector2 Location { get; set; }
        public Vector2 Direction { get; set; } 
        public float Lenght { get; set; }
        public float Width { get; set; }
        public Queue<IRoadElement> Route { get { return this._route; } }
        public CarStateMachine StateMachine { get { return this._stateMachine; } }
    }

}

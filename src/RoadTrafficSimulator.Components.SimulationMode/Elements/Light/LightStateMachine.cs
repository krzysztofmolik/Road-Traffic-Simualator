using System;
using Microsoft.Xna.Framework;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements.Light
{
    public class LightStateMachine
    {
        private Light _light;
        private LightState _state;
        private TimeSpan _lastChanged;

        public LightStateMachine( Light light )
        {
            this._light = light;
            this._state = LightState.Green;
        }

        public LightState State { get { return _state; } }

        public void Update( GameTime gameTime )
        {
            this._lastChanged += gameTime.ElapsedGameTime;
            if ( this._lastChanged > TimeSpan.FromSeconds( 5 ) )
            {
                this._state = ( LightState ) ( ( int ) ++this._state % 3 );
                this._lastChanged = TimeSpan.Zero;
            }
        }
    }
}
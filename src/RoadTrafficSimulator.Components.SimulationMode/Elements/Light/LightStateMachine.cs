using System;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure;
using System.Linq;

namespace RoadTrafficSimulator.Components.SimulationMode.Elements.Light
{
    public class LightStateMachine
    {
        private readonly Light _light;
        private readonly LightTimes _times;
        private LightState _state;
        private TimeSpan _lastChanged;
        private TimeSpan _timer;

        public LightStateMachine( Light light, LightTimes times )
        {
            this._light = light;
            this._times = times;
            this.SetStartupState();
        }

        private void SetStartupState()
        {
            if ( this._times.SetupDealy < TimeSpan.Zero )
            {
                this._state = LightState.Red;
                this._timer = this._times.SetupDealy;
                return;
            }

            var sum = this._times.Times.Sum( s => s.TotalMilliseconds );
            var time = TimeSpan.FromMilliseconds( this._times.SetupDealy.TotalMilliseconds % sum );
            for ( int i = 0; i < this._times.Times.Length; i++ )
            {
                var stateTime = this._times.Times[ i ];
                if ( time < stateTime )
                {
                    this._state = ( LightState ) i;
                    this._timer = time;
                    break;
                }

                time -= stateTime;
            }
        }

        public LightState State { get { return _state; } }

        public void Update( GameTime gameTime )
        {
            this._timer += gameTime.ElapsedGameTime;
            var lightTime = this.GetStateTime( this._state );

            if ( this._timer - this._lastChanged > lightTime )
            {
                this._state = ( LightState ) ( ( int ) ++this._state % 4 );
                this._lastChanged = this._timer;
            }
        }

        private TimeSpan GetStateTime( LightState state )
        {
            switch ( state )
            {
                case LightState.Green:
                    return this._times.GreenLightTime;
                case LightState.Red:
                    return this._times.RedLightTime;
                case LightState.YiellowFromGreen:
                case LightState.YiellowFromRed:
                    return this._times.YellowLightTime;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
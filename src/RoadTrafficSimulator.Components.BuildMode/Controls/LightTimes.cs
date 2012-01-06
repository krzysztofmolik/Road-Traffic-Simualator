using System;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class LightTimes
    {
        private readonly TimeSpan[] _times;

        public LightTimes( TimeSpan setupDelay, TimeSpan red, TimeSpan yellow, TimeSpan green )
        {
            this._times = new TimeSpan[4];
            this.SetupDealy = setupDelay;
            this.RedLightTime = red;
            this.YellowLightTime = yellow;
            this.GreenLightTime = green;
        }

        public TimeSpan SetupDealy { get; set; }

        public TimeSpan RedLightTime
        {
            get { return this._times[ (int) LightState.Red ]; }
            set { this._times[ (int) LightState.Red ] = value; }
        }
        public TimeSpan YellowLightTime
        {
            get { return this._times[ (int) LightState.YiellowFromGreen ]; }
            set
            {
                this._times[ (int) LightState.YiellowFromGreen ] = value;
                this._times[ (int) LightState.YiellowFromRed ] = value;
            }
        }
        public TimeSpan GreenLightTime
        {
            get { return this._times[ (int) LightState.Green ]; }
            set { this._times[ (int) LightState.Green ] = value; }
        }

        public TimeSpan[] Times
        {
            get { return this._times; }
        }
    }
}
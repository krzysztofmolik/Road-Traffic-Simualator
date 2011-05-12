using System;

namespace RoadTrafficSimulator.Infrastructure
{
    public class CameraChangedEventArgs : EventArgs
    {
        public bool Handled { get; set; }
    }
}
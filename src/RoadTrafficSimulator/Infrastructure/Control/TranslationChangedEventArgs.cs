using System;

namespace RoadTrafficSimulator.Infrastructure.Control
{
    public class TranslationChangedEventArgs : EventArgs
    {
        public TranslationChangedEventArgs( IControl control )
        {
            this.Control = control;
        }

        public IControl Control { get; private  set; }
    }
}
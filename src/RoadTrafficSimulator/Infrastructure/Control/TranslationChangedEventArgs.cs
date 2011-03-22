using System;
using System.Diagnostics;

namespace RoadTrafficSimulator.Infrastructure.Control
{
    [DebuggerStepThrough]
    public class TranslationChangedEventArgs : EventArgs
    {
        public TranslationChangedEventArgs( IControl control )
        {
            this.Control = control;
        }

        public IControl Control { get; private  set; }
    }
}
using System;
using Microsoft.Xna.Framework.Input;

namespace RoadTrafficSimulator.Infrastructure
{
    public class KeyboardKeysChangedArgs : EventArgs
    {
        public KeyboardKeysChangedArgs(Keys key, KeyState state)
        {
            Key = key;
            State = state;
        }

        public KeyState State { get; private set; }

        public Keys Key { get; private set; }

        public bool Handled { get; set; }
    }
}
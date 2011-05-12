using Microsoft.Xna.Framework.Input;

namespace RoadTrafficSimulator.Infrastructure
{
    public class KeysState
    {
        public KeysState( Keys key )
        {
            this.Key = key;
        }

        public Keys Key { get; private set; }
    }
}
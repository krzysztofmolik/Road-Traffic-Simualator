using Microsoft.Xna.Framework.Input;

namespace RoadTrafficSimulator
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
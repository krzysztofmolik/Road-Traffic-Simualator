using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Control;
using XnaVs10.Extension;

namespace RoadTrafficSimulator.Extension
{
    public static class ControlExtensions
    {
        public static void SetLocation( this IControl control, Vector2 location)
        {
            var delta = location - control.Location;
            control.Translate(Matrix.CreateTranslation(delta.ToVector3()));
        }
    }
}
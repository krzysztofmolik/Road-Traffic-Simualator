using System.Diagnostics;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Infrastructure.Extension
{
    public static class ControlExtensions
    {
        [DebuggerStepThrough]
        public static void SetLocation( this IControl control, Vector2 location)
        {
            var delta = location - control.Location;
            control.Translate(Matrix.CreateTranslation(delta.ToVector3()));
        }
    }
}
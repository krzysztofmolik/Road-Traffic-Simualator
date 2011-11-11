using Microsoft.Xna.Framework;

namespace RoadTrafficConstructor.Converters
{
    public class VectorToStringConveter : ConveterBase<Vector2, string>
    {
        protected override string Convert( Vector2 value )
        {
            return string.Format("{0} x {1}", value.X, value.Y);
        }
    }
}
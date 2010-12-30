using Microsoft.Xna.Framework;

namespace XnaRoadTrafficConstructor.Infrastucure.Draw
{
    public interface IShape
    {
        Vector2[] ShapePoints { get; }
        Vector2[] DrawableShape { get; }
    }
}
using Microsoft.Xna.Framework;

namespace RoadTrafficSimulator.Infrastructure.Draw
{
    public interface IShape
    {
        Vector2[] ShapePoints { get; }
        Vector2[] DrawableShape { get; }
        int[] Indexes { get; }
    }
}
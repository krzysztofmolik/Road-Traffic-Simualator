using Microsoft.Xna.Framework;
using XnaRoadTrafficConstructor.Infrastucure.Draw;

namespace RoadTrafficSimulator.Road.Controls
{
    public class InvisibleShape : IShape
    {
        public Vector2[] ShapePoints
        {
            get { return new Vector2[0]; }
        }

        public Vector2[] DrawableShape
        {
            get { return new Vector2[0]; }
        }
    }
}
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Draw;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class InvisibleShape : IShape
    {
        public Vector2[] ShapePoints
        {
            get { return new Vector2[ 0 ]; }
        }

        public Vector2[] DrawableShape
        {
            get { return new Vector2[ 0 ]; }
        }

        public short[] Indexes
        {
            get { return new short[ 0 ]; }
        }
    }
}
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Draw;

namespace RoadTrafficSimulator.Infrastructure.Controls
{
    public class Quadrangle : IShape
    {
        public Quadrangle()
        {
            this.ShapePoints = new Vector2[ Corners.Count ];
        }

        public Quadrangle( Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom )
            : this()
        {
            this.LeftTop = leftTop;
            this.RightTop = rightTop;
            this.RightBottom = rightBottom;
            this.LeftBottom = leftBottom;
        }

        public Vector2[] ShapePoints { get; private set; }

        public Vector2 LeftTop
        {
            get { return this.ShapePoints[ Corners.LeftTop ]; }
            set { this.ShapePoints[ Corners.LeftTop ] = value; }
        }

        public Vector2 RightTop
        {
            get { return this.ShapePoints[ Corners.RightTop ]; }
            set { this.ShapePoints[ Corners.RightTop ] = value; }
        }
        public Vector2 RightBottom
        {
            get { return this.ShapePoints[ Corners.RightBottom ]; }
            set { this.ShapePoints[ Corners.RightBottom ] = value; }
        }
        public Vector2 LeftBottom
        {
            get { return this.ShapePoints[ Corners.LeftBottom ]; }
            set { this.ShapePoints[ Corners.LeftBottom ] = value; }
        }

        public Vector2[] DrawableShape
        {
            get { return this.ToTriangleList(); }
        }

        private Vector2[] ToTriangleList()
        {
            return new[]
                       {
                           this.LeftTop,
                           this.LeftBottom,
                           this.RightTop,

                           this.LeftBottom,
                           this.RightBottom,
                           this.RightTop,
                       };
        }
    }
}
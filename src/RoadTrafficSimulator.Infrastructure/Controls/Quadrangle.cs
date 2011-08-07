using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.Draw;
using System.Linq;
using RoadTrafficSimulator.Infrastructure.Extension;
using Common;

namespace RoadTrafficSimulator.Infrastructure.Controls
{
    public class ColorableShape
    {
        private IShape _shape;
        private Color _color;
        private VertexPositionColor[] _vertex;

        public ColorableShape( IShape shape, Color color )
        {
            this._shape = shape;
            this._color = color;
            this.UpdateVertex();
        }

        public void SetColor( Color color )
        {
            if ( color != this._color )
            {
                this._color = color;
                this.UpdateVertex();
            }
        }

        public void SetShape( IShape shape )
        {
            // TODO Do nothing if shape are equal
            this._shape = shape;
            this.UpdateVertex();
        }

        private void UpdateVertex()
        {
            this._vertex = this._shape.ShapePoints.Select( s => new VertexPositionColor( s.ToVector3(), this._color ) ).ToArray();
        }

        public VertexPositionColor[] Vertex { get { return this._vertex; } }
        public int[] Indexes { get { return this._shape.Indexes; } }
    }

    public class Quadrangle : IShape
    {
        private static readonly int[] QuadrangleIndexes = new[] { 0, 3, 1, 2, 1, 3 };

        public static Quadrangle Create( Vector2 location, float width, float height )
        {
            var halfWidth = width / 2;
            var halfHeight = width / 2;

            return new Quadrangle(
                                new Vector2( -halfWidth, -halfHeight ) + location,
                                new Vector2( halfWidth, -halfHeight ) + location,
                                new Vector2( halfWidth, halfHeight ) + location,
                                new Vector2( -halfWidth, halfHeight ) + location );
        }

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

        public int[] Indexes
        {
            get { return QuadrangleIndexes; }
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

        public void Transform( Matrix translationMatrix )
        {
            var newShapePoints = this.ShapePoints.Select( p => Vector2.Transform( p, translationMatrix ) ).ToArray();
            this.ShapePoints = newShapePoints;
        }
    }
}
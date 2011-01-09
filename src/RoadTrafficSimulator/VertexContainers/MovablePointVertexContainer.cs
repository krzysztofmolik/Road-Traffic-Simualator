using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Road.Connectors;
using RoadTrafficSimulator.Road.Controls;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using XnaRoadTrafficConstructor.Road;
using XnaVs10.Extension;

namespace RoadTrafficSimulator.VertexContainers
{
    internal class MovablePointVertexContainer : VertexContainerBase<MovablePoint, VertexPositionColor>
    {
        private Quadrangle _quadrangle;

        public MovablePointVertexContainer( MovablePoint @object )
            : base( @object )
        {
            this.NormalColor = Color.Red;
        }

        public Color NormalColor { get; private set; }

        private Quadrangle CreateQuadrangle()
        {
            var leftTop = this.Object.Location - new Vector2( Constans.PointSize / 2, Constans.PointSize / 2 );
            var rightTop = leftTop + new Vector2( Constans.PointSize, 0 );
            var rightBottom = rightTop + new Vector2( 0, Constans.PointSize );
            var leftBottom = rightBottom - new Vector2( Constans.PointSize, 0 );
            return new Quadrangle( leftTop, rightTop, rightBottom, leftBottom );
        }

        protected override VertexPositionColor[] UpdateShapeAndCreateVertex()
        {
            this._quadrangle = this.CreateQuadrangle();
            return
                this._quadrangle.DrawableShape
                                    .Select(s => new VertexPositionColor(s.ToVector3(), this.NormalColor))
                                    .ToArray();
        }

        public override IShape Shape
        {
            get
            {
                if ( this._quadrangle == null )
                {
                    this._quadrangle = this.CreateQuadrangle();
                }

                return this._quadrangle;
            }
        }

        protected override void DrawControl( Graphic graphic )
        {
            graphic.VertexPositionalColorDrawer.DrawTriangeList( this.Vertex );
        }
    }
}
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Road;
using RoadTrafficSimulator.Road.Controls;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using XnaRoadTrafficConstructor.Road;
using XnaVs10.Extension;

namespace XnaRoadTrafficConstructor.VertexContainers
{
    public class StopLineVertexContainer : VertexContainerBase<StopLine, VertexPositionColor>
    {
        private readonly Color _normalColor = Color.Red;
        private IShape _shape;

        public StopLineVertexContainer( StopLine stopLine )
            : base( stopLine )
        {
        }

        protected override VertexPositionColor[] UpdateShapeAndCreateVertex()
        {
            this._shape = this.CreateShape();
            return this._shape.DrawableShape
                                    .Select( s => new VertexPositionColor( s.ToVector3(), this._normalColor ) )
                                    .ToArray();

        }

        private IShape CreateShape()
        {
            return new Quadrangle( this.Object.LeftTop,
                                  this.Object.RightTop,
                                  this.Object.RightBottom,
                                  this.Object.LeftBottom );
        }

        public override IShape Shape
        {
            get { return this._shape; }
        }

        protected override void DrawControl( Graphic graphic )
        {
            graphic.VertexPositionalColorDrawer.DrawTriangeList( this.Vertex );
        }
    }
}
using System.Linq;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Road.RoadJoiners;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using XnaRoadTrafficConstructor.Road.RoadJoiners;
using XnaVs10.Extension;

namespace XnaRoadTrafficConstructor.VertexContainers
{
    public class MovableRectlangeVertexContainer : VertexContainerBase<MovableRectlange, VertexPositionColor>
    {
        private readonly Color _normalColor = Color.IndianRed;
        private IShape _shape;

        public MovableRectlangeVertexContainer( MovableRectlange @object )
            : base( @object )
        {
        }

        protected override VertexPositionColor[] UpdateShapeAndCreateVertex()
        {
            this._shape = this.CreateShape();
            return this._shape.DrawableShape
                                .Select(s => new VertexPositionColor(s.ToVector3(), this._normalColor))
                                .ToArray();
        }

        public override IShape Shape
        {
            get { return this._shape; }
        }

        protected override void DrawControl( Graphic graphic )
        {
            graphic.VertexPositionalColorDrawer.DrawTriangeList( this.Vertex );
            this.Object.Points.ForEach( p => this.DrawControl( p, graphic ) );
        }

        private IShape CreateShape()
        {
            return new Quadrangle( 
                                   this.Object.LeftTop.Location,
                                   this.Object.RightTop.Location,
                                   this.Object.RightBottom.Location,
                                   this.Object.LeftBottom.Location );
        }

    }
}
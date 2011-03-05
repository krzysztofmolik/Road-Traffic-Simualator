using System.Linq;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.Draw;
using RoadTrafficSimulator.Road;
using RoadTrafficSimulator.Road.Controls;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using XnaRoadTrafficConstructor.Road;
using XnaVs10.Extension;

namespace XnaRoadTrafficConstructor.VertexContainers
{
    public class RoadLaneBlockVertexContainer : VertexContainerBase<IRoadLaneBlock, VertexPositionColor>
    {
        private readonly Color _fillColor = Constans.RoadColor;
        private IShape _shape;

        public RoadLaneBlockVertexContainer( IRoadLaneBlock roadLaneBlock )
            : base( roadLaneBlock )
        {
        }

        protected override VertexPositionColor[] UpdateShapeAndCreateVertex()
        {
            this._shape = this.CreateShape();

            return this._shape.DrawableShape
                                    .Select( s => new VertexPositionColor( s.ToVector3(), this._fillColor ) )
                                    .ToArray();
        }

        private IShape CreateShape()
        {
            return new Quadrangle(
                                this.Object.LeftTopLocation,
                                this.Object.RightTopLocation,
                                this.Object.RightBottomLocation,
                                this.Object.LeftBottomLocation );
        }

        public override IShape Shape
        {
            get { return this._shape; }
        }

        protected override void DrawControl( Graphic graphic )
        {
            graphic.VertexPositionalColorDrawer.DrawTriangeList( this.Vertex );
            this.DrawControl( this.Object.LeftEdge, graphic );
            this.DrawControl( this.Object.TopEdge, graphic );
            this.DrawControl( this.Object.RightEdge, graphic );
            this.DrawControl( this.Object.BottomEdge, graphic );
        }
    }
}
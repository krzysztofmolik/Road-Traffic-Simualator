using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Draw;
using RoadTrafficSimulator.Infrastructure.Extension;
using RoadTrafficSimulator.Infrastructure.MathHelpers;

namespace RoadTrafficSimulator.Components.BuildMode.VertexContainers
{
    public class SideRoadLaneEdgeVertexContainer : VertexContainerBase<SideRoadLaneEdge, VertexPositionColor>
    {
        private Quadrangle _quadrangle;

        public SideRoadLaneEdgeVertexContainer( SideRoadLaneEdge edge )
            : base( edge, new Color( 90, 90, 90 ) )
        {
        }

        public override IShape Shape
        {
            get { return this._quadrangle; }
        }

        protected override void DrawControl( Graphic graphic )
        {
            graphic.VertexPositionalColorDrawer.DrawTriangeList( this.Vertex );
        }

        protected override VertexPositionColor[] UpdateShapeAndCreateVertex()
        {
            this._quadrangle = this.CreateQuatrangle();
            var vertex = this.Shape.DrawableShape;

            return vertex.Select( v => new VertexPositionColor( v.ToVector3(), this.Color ) )
                .ToArray();
        }

        private Quadrangle CreateQuatrangle()
        {
            var startLine = MyMathHelper.CreatePerpendicualrLine(
                                                                  this.Object.StartLocation,
                                                                  this.Object.EndLocation,
                                                                  Constans.PointSize );
            var endLine = MyMathHelper.CreatePerpendicualrLine(
                                                                this.Object.EndLocation,
                                                                this.Object.StartLocation,
                                                                Constans.PointSize );
            return new Quadrangle(
                            startLine.Item1,
                            startLine.Item2,
                            endLine.Item1,
                            endLine.Item2 );
        }
    }
}
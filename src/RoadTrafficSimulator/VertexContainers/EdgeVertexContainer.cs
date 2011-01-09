using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Road.Controls;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using XnaRoadTrafficConstructor.Road;
using XnaVs10.Extension;
using XnaVs10.MathHelpers;

namespace RoadTrafficSimulator.VertexContainers
{
    public class EdgeVertexContainer : VertexContainerBase<Edge, VertexPositionColor>
    {
        private readonly Color _normalColor = new Color( 90, 90, 90 );
        private readonly Color _selectedColor = Color.Blue;
        private Quadrangle _quadrangle;

        public EdgeVertexContainer( Edge edge )
            : base( edge )
        {
        }

        public override IShape Shape
        {
            get { return this._quadrangle; }
        }

        protected override void DrawControl( Graphic graphic )
        {
            graphic.VertexPositionalColorDrawer.DrawTriangeList( this.Vertex );
            this.Object.StartPoint.VertexContainer.Draw( graphic );
            this.Object.EndPoint.VertexContainer.Draw( graphic );
        }

        protected override VertexPositionColor[] UpdateShapeAndCreateVertex()
        {
            this._quadrangle = this.CreateQuatrangle();
            var vertex = this.Shape.DrawableShape;

            var color = this.GetColor();
            return vertex.Select( v => new VertexPositionColor( v.ToVector3(), color ) )
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

        private Color GetColor()
        {
            return this.Object.IsSelected ? this._selectedColor : this._normalColor;
        }
    }
}
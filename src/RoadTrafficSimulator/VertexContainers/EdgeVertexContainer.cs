using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Road.RoadJoiners;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using XnaRoadTrafficConstructor.Road;
using XnaRoadTrafficConstructor.Road.RoadJoiners;
using XnaVs10.Extension;
using XnaVs10.MathHelpers;

namespace RoadTrafficSimulator.VertexContainers
{
    public class EdgeVertexContainer : VertexContainerBase<EdgeBase, VertexPositionColor>
    {
        private readonly Color _normalColor = new Color( 90, 90, 90 );
        private readonly Color _selectedColor = Color.Blue;
        private Quadrangle _quadrangle;

        public EdgeVertexContainer( EdgeBase edge )
            : base( edge )
        {
        }

        private Color GetColor()
        {
            return this.Object.IsSelected ? this._selectedColor : this._normalColor;
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
                var startLine = MyMathHelper.CreatePerpendicualrLine( this.Object.StartLocation,
                                                                     this.Object.EndLocation,
                                                                     Constans.PointSize );
                var endLine = MyMathHelper.CreatePerpendicualrLine( this.Object.EndLocation,
                                                                   this.Object.StartLocation,
                                                                   Constans.PointSize );
                return new Quadrangle( startLine.Item1,
                                       startLine.Item2,
                                       endLine.Item1,
                                       endLine.Item2 );
        }

        public override IShape Shape
        {
            get { return this._quadrangle; }
        }

        protected override void DrawControl( Graphic graphic )
        {
            graphic.VertexPositionalColorDrawer.DrawTriangeList( this.Vertex );
        }
    }
}
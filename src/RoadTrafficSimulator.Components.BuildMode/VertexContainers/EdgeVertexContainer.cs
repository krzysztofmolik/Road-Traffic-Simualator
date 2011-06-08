﻿using System.Linq;
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
    public class EdgeVertexContainer : VertexContainerBase<Edge, VertexPositionColor>
    {
        private readonly Style _style;
        private Quadrangle _quadrangle;

        public EdgeVertexContainer( Edge edge, Style style)
            : base( edge )
        {
            this._style = style;
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

        protected virtual Color GetColor()
        {
            return this.Object.IsSelected ? this._style.SelectionColor : this._style.NormalColor;
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
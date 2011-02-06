using System;
using System.Linq;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Road.Controls;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using XnaRoadTrafficConstructor.Road;
using XnaVs10.Extension;

namespace RoadTrafficSimulator.VertexContainers
{
    public class RoadJunctionBlockVertexContainer : VertexContainerBase<IRoadJunctionBlock, VertexPositionColor>
    {
        private readonly Color _fillColor = Constans.RoadColor;
        private Quadrangle _shape;

        public RoadJunctionBlockVertexContainer( IRoadJunctionBlock block ) 
            : base(block)
        {
        }

        private Quadrangle CreateShape()
        {
            return new Quadrangle(
                                  this.Object.LeftTopLocation,
                                  this.Object.RightTopLocation,
                                  this.Object.RightBottomLocation,
                                  this.Object.LeftBottomLocation);
        }

        protected override VertexPositionColor[] UpdateShapeAndCreateVertex()
        {
            this._shape = this.CreateShape();

            return this._shape.DrawableShape
                                .Select(s => new VertexPositionColor(s.ToVector3(), this.GetColor() ))
                                .ToArray();
        }

        private Color GetColor()
        {
            return this.Object.IsSelected ? Style.SelectionColor : this._fillColor;
        }

        public override IShape Shape
        {
            get { return this._shape; }
        }

        protected override void DrawControl(Graphic graphic)
        {
            graphic.VertexPositionalColorDrawer.DrawTriangeList( this.Vertex );
            this.Object.RoadJunctionEdges.ForEach( s => s.VertexContainer.Draw( graphic ) );
            this.Object.Points.ForEach( s => s.VertexContainer.Draw( graphic ));
        }
    }
}
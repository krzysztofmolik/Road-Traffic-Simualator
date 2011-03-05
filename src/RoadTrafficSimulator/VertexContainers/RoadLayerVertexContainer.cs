using System;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.Draw;
using RoadTrafficSimulator.Road;
using RoadTrafficSimulator.Road.Controls;
using XnaRoadTrafficConstructor.Infrastucure.Draw;

namespace RoadTrafficSimulator.VertexContainers
{
    public class RoadLayerVertexContainer : VertexContainerBase<RoadLayer, VertexPositionColor>
    {
        private readonly InvisibleShape _shape;

        public RoadLayerVertexContainer( RoadLayer @object )
            : base( @object )
        {
            this._shape = new InvisibleShape();
        }

        public override IShape Shape
        {
            get { return this._shape; }
        }

        protected override VertexPositionColor[] UpdateShapeAndCreateVertex()
        {
            return new VertexPositionColor[ 0 ];
        }

        protected override void DrawControl( Graphic graphic )
        {
            foreach ( var child in this.Object.Children )
            {
                child.VertexContainer.Draw( graphic );
            }
        }
    }
}
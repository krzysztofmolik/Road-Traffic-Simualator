using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Draw;

namespace RoadTrafficSimulator.Components.BuildMode.VertexContainers
{
    public class RoadLayerVertexContainer : VertexContainerBase<RoadLayer, VertexPositionColor>
    {
        private readonly InvisibleShape _shape;

        public RoadLayerVertexContainer( RoadLayer @object )
            : base( @object, Color.Transparent )
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
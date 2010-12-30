using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Road;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using XnaRoadTrafficConstructor.Road.RoadJoiners;

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
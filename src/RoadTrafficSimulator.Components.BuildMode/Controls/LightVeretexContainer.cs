using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Draw;
using RoadTrafficSimulator.Infrastructure.Extension;
using RoadTrafficSimulator.Infrastructure.Textures;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class LightVeretexContainer : VertexContainerBase<LightBlock, VertexPositionTexture>
    {
        private readonly CachedTexture _texture;
        private static readonly float Width = Constans.ToVirtualUnit( 0.7f );
        private static readonly float Height = Constans.ToVirtualUnit( 1.5f );
        private Quadrangle _shape;

        public LightVeretexContainer( LightBlock @object, CachedTexture texture )
            : base( @object, Color.Transparent )
        {
            this._texture = texture;
        }

        public override IShape Shape
        {
            get { return this._shape; }
        }

        protected override VertexPositionTexture[] UpdateShapeAndCreateVertex()
        {
            this._shape = this.CreateQuatrangle();
            return new[]
                       {
                           new VertexPositionTexture(this._shape.LeftTop.ToVector3(), new Vector2(0, 1)),
                           new VertexPositionTexture(this._shape.RightTop.ToVector3(), new Vector2(1, 1)),
                           new VertexPositionTexture(this._shape.RightBottom.ToVector3(), new Vector2(1, 0)),
                           new VertexPositionTexture(this._shape.LeftBottom.ToVector3(), new Vector2(0, 0)),
                       };
        }

        private Quadrangle CreateQuatrangle()
        {
            return new Quadrangle(
                new Vector2( -Width / 2, -Height / 2 ) + this.Object.Location,
                new Vector2( Width / 2, -Height / 2 ) + this.Object.Location,
                new Vector2( Width / 2, Height / 2 ) + this.Object.Location,
                new Vector2( -Width / 2, Height / 2 ) + this.Object.Location );
        }

        protected override void DrawControl( Graphic graphic )
        {
            graphic.VertexPositionalTextureDrawer.DrawIndexedTraingeList( this._texture.Textrue, this.Vertex, this._shape.Indexes );
        }
    }
}
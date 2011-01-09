using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Road;
using RoadTrafficSimulator.Road.Controls;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using XnaRoadTrafficConstructor.Road;
using XnaVs10.Extension;

namespace XnaRoadTrafficConstructor.VertexContainers
{
    public class LigthLocationVertexContainer : VertexContainerBase<LightsLocation, VertexPositionTexture>
    {
        private const string LightTexture = "LightTexture";

        private Texture2D _texture;
        private Quadrangle _shape;

        public LigthLocationVertexContainer( LightsLocation lightsLocation )
            : base( lightsLocation )
        {
        }

        protected override VertexPositionTexture[] UpdateShapeAndCreateVertex()
        {
            this._shape = this.CreateQuadrangle();

            return new[]
                              {
                                  new VertexPositionTexture( this._shape.LeftTop.ToVector3(), new Vector2( 0, 0 ) ),
                                  new VertexPositionTexture( this._shape.RightTop.ToVector3(), new Vector2( 1, 0 ) ),
                                  new VertexPositionTexture( this._shape.LeftBottom.ToVector3(), new Vector2( 0, 1 ) ),

                                  new VertexPositionTexture( this._shape.RightTop.ToVector3(), new Vector2( 1, 0 ) ), 
                                  new VertexPositionTexture( this._shape.RightBottom.ToVector3(), new Vector2( 1, 1 ) ), 
                                  new VertexPositionTexture( this._shape.LeftBottom.ToVector3(), new Vector2( 0, 1 ) ), 
                              };
        }

        private Quadrangle CreateQuadrangle()
        {
            return new Quadrangle(this.Object.Shape[0],
                                  this.Object.Shape[1],
                                  this.Object.Shape[2],
                                  this.Object.Shape[3]);
        }

        public override IShape Shape
        {
            get { return this._shape; }
        }

        protected override void DrawControl( Graphic graphic )
        {
            if ( this._texture == null )
            {
                this._texture = graphic.ContentManager.Load<Texture2D>( LightTexture );
            }

            graphic.VertexPositionalTextureDrawer.DrawTriangeList( this._texture, this.Vertex );
        }
    }
}
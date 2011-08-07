using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Infrastructure.Textures
{
    public class TextureDeclaration
    {
        public static TextureDeclaration Create( string assetName, float x, float y, float rightBottomX, float rightBottomY )
        {
            return new TextureDeclaration( assetName, new Vector2( x, y ), new Vector2( rightBottomX, y ), new Vector2( rightBottomX, rightBottomY ), new Vector2( x, rightBottomY ) );

        }
        public TextureDeclaration( string assetName, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom )
        {
            this.Quadrangle = new Quadrangle( leftTop, rightTop, rightBottom, leftBottom );
            this.Name = assetName;

        }

        public Quadrangle Quadrangle { get; private set; }
        public string Name { get; private set; }
    }
}
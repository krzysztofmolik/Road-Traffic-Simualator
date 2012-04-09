using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Extension;

namespace RoadTrafficSimulator.Infrastructure.Textures
{
    public class TextureInPoint
    {
        private CachedTexture _texture;
        private readonly Quadrangle _quadrangle;
        private float _angel;
        private VertexPositionTexture[] _blockses;
        private Vector2 _location;
        private float _width;
        private float _heigth;

        public TextureInPoint( CachedTexture texture, Quadrangle quadrangle )
        {
            this._angel = 0.0f;
            this._texture = texture;
            this._quadrangle = quadrangle;
            this.CalculateBlocks();
            this._width = 0.01f;
            this._heigth = 0.01f;
        }

        private void CalculateBlocks()
        {
            var leftTop = new Vector2( this._location.X - this._width / 2.0f, this._location.Y - this._heigth / 2.0f );
            var rightTop = leftTop + new Vector2( this._width, 0 );
            var rightBottom = leftTop + new Vector2( this._width, this._heigth );
            var leftBottom = leftTop + new Vector2( 0, this._heigth );
            this._blockses = new[]
                                 {
                                     new VertexPositionTexture( leftTop.ToVector3(), this._quadrangle.LeftTop ),
                                     new VertexPositionTexture( rightTop.ToVector3(), this._quadrangle.RightTop ),
                                     new VertexPositionTexture( rightBottom.ToVector3(), this._quadrangle.RightBottom ),
                                     new VertexPositionTexture( leftBottom.ToVector3(), this._quadrangle.LeftBottom )
                                 };
        }

        public void SetAngel( float angel )
        {
            var vectorDiff = angel - this._angel;
            var rotationMatrix = Matrix.CreateRotationZ( vectorDiff );
            this._quadrangle.Transform( rotationMatrix );
            this.CalculateBlocks();
        }

        public void SetLoactoin( Vector2 point )
        {
            this._location = point;
        }

        public Texture2D Texture
        {
            get { return this._texture.Textrue; }
        }

        public VertexPositionTexture[] Blocks
        {
            get { return this._blockses; }
        }

        public short[] Indexes
        {
            get { return this._quadrangle.Indexes; }
        }

        public void SetWidth( float width )
        {
            this._width = width;
            this.CalculateBlocks();
        }

        public void SetHeigth( float height )
        {
            this._heigth = height;
            this.CalculateBlocks();
        }
    }
}
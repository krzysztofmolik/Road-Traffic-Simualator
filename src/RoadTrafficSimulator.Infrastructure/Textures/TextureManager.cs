using System.Collections.Generic;
using RoadTrafficSimulator.Infrastructure.DependencyInjection;

namespace RoadTrafficSimulator.Infrastructure.Textures
{
    public class TextureManager
    {
        private readonly IContentManagerAdapter _contentManager;
        private readonly Dictionary<TextureType, TextureDeclaration> _textureInPoint;

        public TextureManager( IContentManagerAdapter contentManager )
        {
            this._contentManager = contentManager;
            this._textureInPoint = new Dictionary<TextureType, TextureDeclaration>();
            // TODO Here is something wrong
            this._textureInPoint.Add( TextureType.LeftArrow, TextureDeclaration.Create( "arrows", 0.0f, 0.0f, 0.25f, 1.0f ) );
            this._textureInPoint.Add( TextureType.DownArrow, TextureDeclaration.Create( "arrows", 0.25f, 0.0f, 0.5f, 1.0f ) );
            this._textureInPoint.Add( TextureType.RightArrow, TextureDeclaration.Create( "arrows", 0.5f, 0.0f, 0.75f, 1.0f ) );
            this._textureInPoint.Add( TextureType.UpArrow, TextureDeclaration.Create( "arrows", 0.75f, 0.0f, 1.0f, 1.0f ) );
        }

        public TextureInPoint GetTextureInPoint( TextureType textureType )
        {
            var textureDeclartaion = this._textureInPoint[ textureType ];
            return new TextureInPoint( this._contentManager.Load( textureDeclartaion.Name ), textureDeclartaion.Quadrangle );
        }
    }

    public enum TextureType
    {
        LeftArrow,
        UpArrow,
        RightArrow,
        DownArrow,
    }
}
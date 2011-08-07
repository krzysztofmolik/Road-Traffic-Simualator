using System.Collections.Generic;
using Common;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.Messages;
using RoadTrafficSimulator.Infrastructure.Textures;

namespace RoadTrafficSimulator.Infrastructure.DependencyInjection
{
    public class ContentManagerAdapter : IContentManagerAdapter
    {
        private readonly ContentManager _contentManager;
        private Dictionary<string, CachedTexture> _textures;

        public ContentManagerAdapter( ContentManager contentManager, IEventAggregator eventAggregator )
        {
            this._contentManager = contentManager;
            this._textures = new Dictionary<string, CachedTexture>();
        }

        public CachedTexture Load( string assetName )
        {
            var texture = default( CachedTexture );
            if ( this._textures.TryGetValue( assetName, out texture ) )
            {
                return texture;
            }

            texture = new CachedTexture( assetName ) { Textrue = this._contentManager.Load<Texture2D>( assetName ) };
            this._textures.Add( assetName, texture );
            return texture;
        }

        public void ReloadAllTextures()
        {
            foreach ( var texture in this._textures.Values )
            {
                texture.Textrue = this._contentManager.Load<Texture2D>( texture.AssetName );
            }
        }

        public void Unload( UnloadConntent message )
        {
            this._contentManager.Unload();
        }
    }
}
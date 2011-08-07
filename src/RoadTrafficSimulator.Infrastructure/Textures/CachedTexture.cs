using Microsoft.Xna.Framework.Graphics;

namespace RoadTrafficSimulator.Infrastructure.Textures
{
    public class CachedTexture
    {
        public CachedTexture( string assetName )
        {
            this.AssetName = assetName;
        }

        public string AssetName { get; private set; }

        public Texture2D Textrue { get; set; }
    }
}
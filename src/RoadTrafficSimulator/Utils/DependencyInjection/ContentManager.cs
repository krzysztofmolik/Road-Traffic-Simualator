using Microsoft.Xna.Framework.Content;
using XnaRoadTrafficConstructor.Utils.DependencyInjection;

namespace RoadTrafficSimulator.Utils.DependencyInjection
{
    internal class ContentManagerAdapter : IContentManager
    {
        private readonly ContentManager _contentManager;

        public ContentManagerAdapter( ContentManager contentManager )
        {
            this._contentManager = contentManager;
        }

        public TAssetType Load<TAssetType>( string assetName )
        {
            return this._contentManager.Load<TAssetType>( assetName );
        }
    }
}
using Common;
using Microsoft.Xna.Framework.Content;
using RoadTrafficSimulator.Infrastructure.Messages;

namespace RoadTrafficSimulator.Infrastructure.DependencyInjection
{
    internal class ContentManagerAdapter : IContentManager, IHandle<UnloadConntent>
    {
        private readonly ContentManager _contentManager;

        public ContentManagerAdapter( ContentManager contentManager, IEventAggregator eventAggregator )
        {
            this._contentManager = contentManager;
            eventAggregator.Subscribe( this );
        }

        public TAssetType Load<TAssetType>( string assetName )
        {
            return this._contentManager.Load<TAssetType>( assetName );
        }

        public void Handle( UnloadConntent message )
        {
            this._contentManager.Unload();
        }
    }
}
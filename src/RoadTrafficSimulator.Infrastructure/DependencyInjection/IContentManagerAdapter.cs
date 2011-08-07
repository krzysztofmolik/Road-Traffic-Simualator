using RoadTrafficSimulator.Infrastructure.Messages;
using RoadTrafficSimulator.Infrastructure.Textures;

namespace RoadTrafficSimulator.Infrastructure.DependencyInjection
{
    public interface IContentManagerAdapter 
    {
        CachedTexture Load( string assetName );
        void ReloadAllTextures();
        void Unload( UnloadConntent message );
    }
}
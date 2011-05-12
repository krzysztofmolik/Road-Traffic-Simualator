namespace RoadTrafficSimulator.Infrastructure.DependencyInjection
{
    public interface IContentManager
    {
        TAssetType Load<TAssetType>( string assetName );
    }
}
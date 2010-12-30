namespace XnaRoadTrafficConstructor.Utils.DependencyInjection
{
    public interface IContentManager
    {
        TAssetType Load<TAssetType>( string assetName );
    }
}
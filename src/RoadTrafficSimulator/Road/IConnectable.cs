namespace RoadTrafficSimulator.Road
{
    public interface IConnectable<TJoiner>
    {
        void NotifyAboutConnection( IConnectable<TJoiner> conetableBase );
        void NotifyAboutDisconnected( IConnectable<TJoiner> connectable );
    }

    public interface IConnectable<out TOwner, TJoiner> : IConnectable<TJoiner>
    {
        TOwner Owner { get; }
    }
}
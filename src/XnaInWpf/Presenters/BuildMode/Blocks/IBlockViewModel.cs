namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks
{
    public interface IBlockViewModel
    {
        object Preview { get;  }
        string Name { get; }
        void GoBack();
        void Execute();
    }
}
namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks
{
    public class ChangeBlock
    {
        public ChangeBlock( IBlockViewModel mainBlock )
        {
            this.Block = mainBlock;
        }

        public IBlockViewModel Block { get; private set; }
    }
}
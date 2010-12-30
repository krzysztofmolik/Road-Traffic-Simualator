using System;
using System.Collections.Generic;
using System.Linq;
using XnaInWpf.Presenters.Blocks;

namespace RoadTrafficConstructor.Presenters.Blocks
{
    public class BlockManager : IBlockManager
    {
        private readonly IEnumerable<IBlockViewModel> _allBlocks;

        public BlockManager( IEnumerable<IBlockViewModel> allBlocks )
        {
            this._allBlocks = allBlocks;
        }

        public IEnumerable<IBlockViewModel> GetRootParrents()
        {
            return this.GetBlockOwnerBy( null );
        }

        public IEnumerable<IBlockViewModel> GetBlockOwnerBy( Type owner )
        {
            var trees = this._allBlocks.Where( obj => obj.Parent == owner );
            foreach( var block in trees )
            {
                block.AvailableBlocks = this.GetBlockOwnerBy( block.GetType() );
            }

            return trees;
        }
    }
}
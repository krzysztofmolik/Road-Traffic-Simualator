using System;
using System.Collections.Generic;
using RoadTrafficConstructor.Presenters.Blocks;

namespace XnaInWpf.Presenters.Blocks
{
    public interface IBlockManager
    {
        IEnumerable<IBlockViewModel> GetRootParrents();
        IEnumerable<IBlockViewModel> GetBlockOwnerBy( Type owner );
    }
}
using System;
using System.Collections.Generic;
using RoadTrafficSimulator.Road;

namespace RoadTrafficConstructor.Presenters.Blocks
{
    public interface IBlockViewModel
    {
        Type Parent { get; }
        string Name { get; }
        bool IsTree { get; }
        IEnumerable<IBlockViewModel> AvailableBlocks { get; set; }
        void Execute( BuilderControl builderControl );
    }
}
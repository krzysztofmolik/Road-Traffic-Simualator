using System;
using System.Collections.Generic;
using RoadTrafficSimulator.Components.BuildMode.Commands;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks
{
    public interface IBlockViewModel
    {
        Type Parent { get; }
        string Name { get; }
        bool IsTree { get; }
        IEnumerable<IBlockViewModel> AvailableBlocks { get; set; }
        void Execute( Action<CommandType> executeCommand );
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RoadTrafficSimulator.Components.BuildMode;
using RoadTrafficSimulator.Road;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.Junctions
{
    public class RoadJunctionBlockViewModelModel : IBlockViewModel
    {
        public Type Parent
        {
            get { return null; }
        }

        public string Name
        {
            get { return "Junction block"; }
        }

        public bool IsTree
        {
            get { return false; }
        }

        public IEnumerable<IBlockViewModel> AvailableBlocks
        {
            get { return null; }
            set { Debug.Assert( value.Count() == 0, "value.Count() == 0" ); }
        }

        public void Execute( BuilderControl builderControl )
        {
            builderControl.AddingRoadJunctionBlock = true;
        }
    }
}
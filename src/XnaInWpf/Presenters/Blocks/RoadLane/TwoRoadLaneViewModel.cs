using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RoadTrafficSimulator.Integration;
using RoadTrafficSimulator.Road;

namespace RoadTrafficConstructor.Presenters.Blocks.RoadLane
{
    public class TwoRoadLaneViewModel : IBlockViewModel
    {
        public Type Parent
        {
            get { return typeof( RoadLaneFolderViewModel ); }
        }

        public string Name
        {
            get { return "Two lane"; }
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
            builderControl.AddingRoadLane = true;
        }
    }
}
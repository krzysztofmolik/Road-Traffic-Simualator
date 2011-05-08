﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RoadTrafficSimulator.Road;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.RoadLane
{
    public class OneRoadLaneViewModel : IBlockViewModel
    {
        public Type Parent
        {
            get { return null; }
        }

        public string Name
        {
            get { return "One lane"; }
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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RoadTrafficSimulator.Road;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.Lights
{
    public class StandardLightViewModel : IBlockViewModel
    {
        public Type Parent { get { return typeof( LightsViewModel ); } }

        public string Name { get { return "Standard light"; } }

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
            throw new NotImplementedException();
        }
    }
}
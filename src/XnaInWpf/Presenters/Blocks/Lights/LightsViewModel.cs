using System;
using System.Collections.Generic;
using RoadTrafficSimulator.Road;
using XnaInWpf.Presenters;

namespace RoadTrafficConstructor.Presenters.Blocks.Lights
{
    public class LightsViewModel : MyModelBase, IBlockViewModel
    {
        private IEnumerable<IBlockViewModel> _availableBlocks;

        public bool IsTree
        {
            get { return true; }
        }

        public IEnumerable<IBlockViewModel> AvailableBlocks
        {
            get { return this._availableBlocks; }
            set
            {
                this._availableBlocks = value;
                this.OnPropertyChanged(() => this.AvailableBlocks);
            }
        }

        public void Execute( BuilderControl builderControl )
        {
            throw new NotSupportedException();
        }

        public Type Parent
        {
            get { return null; }
        }

        public string Name
        {
            get { return "Lights"; }
        }
    }
}
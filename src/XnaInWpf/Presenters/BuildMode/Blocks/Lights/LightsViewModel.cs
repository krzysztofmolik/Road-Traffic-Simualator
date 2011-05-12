using System;
using System.Collections.Generic;
using System.ComponentModel;
using Common.Wpf;
using RoadTrafficSimulator.Components.BuildMode;
using RoadTrafficSimulator.Road;

namespace RoadTrafficConstructor.Presenters.BuildMode.Blocks.Lights
{
    public class LightsViewModel : IBlockViewModel, INotifyPropertyChanged
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
                this.PropertyChanged.Raise( this, t => t.AvailableBlocks);
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

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
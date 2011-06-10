using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using Common;
using Common.Wpf;
using RoadTrafficConstructor.Presenters.BuildMode.Blocks;
using RoadTrafficSimulator.Components.BuildMode.Messages;
using XnaInWpf.Presenters.Blocks;

namespace RoadTrafficConstructor.Presenters.BuildMode
{
    public class BuildJunctionViewModel : INotifyPropertyChanged
    {
        private IBlockViewModel _selectedItem;
        private IEnumerable<IBlockViewModel> _blocks;
        private readonly IBlockManager _blockManager;
        private readonly IEventAggregator _eventAggregator;

        public BuildJunctionViewModel( IBlockManager blockManager, IEventAggregator eventAggregator )
        {
            Contract.Requires( blockManager != null );
            Contract.Requires( eventAggregator != null );
            Contract.Ensures( this.Blocks.Any() );

            this._blockManager = blockManager;
            this._eventAggregator = eventAggregator;
            this.Blocks = this._blockManager.GetRootParrents();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<IBlockViewModel> Blocks
        {
            get { return this._blocks; }
            set
            {
                this._blocks = value;
                this.PropertyChanged.Raise( this, x => x.Blocks );
            }
        }

        public IBlockViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                this._selectedItem = value;
                this.PropertyChanged.Raise( this, x => x.SelectedItem );
            }
        }

        public void ChooseBlock()
        {
            if ( this.SelectedItem == null )
            {
                return;
            }

            if ( this.SelectedItem.IsTree )
            {
                this.Blocks = this.SelectedItem.AvailableBlocks;
            }
            else
            {
                this.SelectedItem.Execute( command => this._eventAggregator.Publish( new ExecuteCommand( command ) ) );
            }
        }

        public void GoBack()
        {
            this.Blocks = this._blockManager.GetRootParrents();
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Common;
using Common.Wpf;
using RoadTrafficConstructor.Presenters.BuildMode.Blocks;

namespace RoadTrafficConstructor.Presenters.BuildMode
{
    public class BuildJunctionViewModel : INotifyPropertyChanged, IHandle<ChangeBlock>
    {
        private IBlockViewModel _selectedItem;
        private IEnumerable<IBlockViewModel> _blocks;
        private readonly IEventAggregator _eventAggregator;

        public BuildJunctionViewModel( IEventAggregator eventAggregator )
        {
            this._eventAggregator = eventAggregator;
            this.SelectedItem = new MainBlockViewModel( this._eventAggregator );
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IBlockViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                this._selectedItem = value;
                this.PropertyChanged.Raise( this, x => x.SelectedItem );
            }
        }

        public void GoBack()
        {
            if ( this.SelectedItem != null )
            {
                this.SelectedItem.GoBack();
            }
        }

        public void Handle(ChangeBlock message)
        {
            if ( message.Block != null )
            {
                this.SelectedItem = message.Block;
            }
        }
    }
}
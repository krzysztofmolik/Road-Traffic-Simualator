using System;
using System.Collections.Generic;
using RoadTrafficConstructor.Presenters.Blocks;
using RoadTrafficSimulator.Integration;
using RoadTrafficSimulator.Road;
using XnaInWpf.Presenters;
using XnaInWpf.Presenters.Blocks;
using XnaInWpf.Presenters.Interfaces;

namespace RoadTrafficConstructor.Presenters
{
    public class ShellViewModel : MyModelBase, IShellViewModel
    {
        private readonly IBlockManager _blockManager;
        private readonly Func<IMouseInformationModel> _mouseInformationFactory;

        private IEnumerable<IBlockViewModel> _blocks;
        private IBlockViewModel _selectedItem;
        private IMouseInformationModel _mouseInformation;
        private bool _shouldShowStopLine;
        private bool _showRoadDirection;
        private readonly BuilderControl _builderControl;

        public ShellViewModel( IBlockManager blockManager, Func<IMouseInformationModel> mouseInformationFactory, BuilderControl builderControl )
        {
            this._blockManager = blockManager;
            this._builderControl = builderControl;
            this._mouseInformationFactory = mouseInformationFactory;
            this.Blocks = blockManager.GetRootParrents();
        }

        public void OnLoaded()
        {
            this.MouseInformationModel = this._mouseInformationFactory();
        }

        public IMouseInformationModel MouseInformationModel
        {
            get { return this._mouseInformation; }
            set
            {
                this._mouseInformation = value;
                this.OnPropertyChanged( () => MouseInformationModel );
            }
        }

        public bool ShouldShowStopLine
        {
            get { return this._shouldShowStopLine; }
            set
            {
                this._shouldShowStopLine = value;
                this.OnPropertyChanged( () => this.ShouldShowStopLine );
                this._builderControl.ShowStopLine = value;
            }
        }

        public bool ShowRoadDirection
        {
            get { return this._showRoadDirection; }
            set
            {
                this._showRoadDirection = value;
                this.OnPropertyChanged( () => this.ShowRoadDirection );
                this._builderControl.ShowRoadDirection = value;
            }
        }

        public IEnumerable<IBlockViewModel> Blocks
        {
            get { return this._blocks; }
            set
            {
                this._blocks = value;
                this.OnPropertyChanged( () => this.Blocks );
            }
        }

        public IBlockViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                this._selectedItem = value;
                this.OnPropertyChanged( () => this.SelectedItem );
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
                this.SelectedItem.Execute( this._builderControl );
            }
        }

        public void GoBack()
        {
            this.Blocks = this._blockManager.GetRootParrents();
        }
    }
}
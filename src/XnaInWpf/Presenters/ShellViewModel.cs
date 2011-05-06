using System;
using System.Collections.Generic;
using Autofac;
using Common;
using RoadTrafficConstructor.Presenters.Blocks;
using RoadTrafficSimulator.Road;
using RoadTrafficSimulator.RoadTrafficSimulatorMessages;
using XnaInWpf.Presenters;
using XnaInWpf.Presenters.Blocks;
using XnaInWpf.Presenters.Interfaces;

namespace RoadTrafficConstructor.Presenters
{
    public class ShellViewModel : MyModelBase, IShellViewModel, IHandle<XnaWindowInitialized>
    {
        private readonly IBlockManager _blockManager;
        private readonly Func<IMouseInformationModel> _mouseInformationFactory;
        private readonly BuilderControl _builderControl;
        private readonly IEventAggregator _eventAggreator;
        private readonly ILifetimeScope _container;
        private WorldController _wordController;

        private IEnumerable<IBlockViewModel> _blocks;
        private IBlockViewModel _selectedItem;
        private IMouseInformationModel _mouseInformation;
        private bool _shouldShowStopLine;
        private bool _showRoadDirection;

        public ShellViewModel(
            IBlockManager blockManager,
            Func<IMouseInformationModel> mouseInformationFactory,
            BuilderControl builderControl,
            IEventAggregator eventAggreator,
            ILifetimeScope container )
        {
            this._blockManager = blockManager;
            this._container = container;
            this._eventAggreator = eventAggreator;
            this._builderControl = builderControl;
            this._mouseInformationFactory = mouseInformationFactory;
            this.Blocks = blockManager.GetRootParrents();

            this._eventAggreator.Subscribe( this );
            this.ServiceProvider = this._container.Resolve<IServiceProvider>();
        }

        protected IServiceProvider ServiceProvider { get; set; }

        public void Initialize( WorldController worldController )
        {
            this.MouseInformationModel = this._mouseInformationFactory();
            this._wordController = worldController.NotNull();
        }


        public void IncreaseZoom()
        {
            if ( this._wordController == null )
            {
                return;
            }

            var zoomValue = this._wordController.GetZoom();
            this._wordController.SetZoom( zoomValue + 0.1f );
        }

        public void DecreaseZoom()
        {
            if ( this._wordController == null )
            {
                return;
            }

            var zoomValue = this._wordController.GetZoom();
            this._wordController.SetZoom( zoomValue - 0.1f );
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

        public void Handle( XnaWindowInitialized message )
        {
            this._wordController = this._container.Resolve<WorldController>();
        }
    }
}
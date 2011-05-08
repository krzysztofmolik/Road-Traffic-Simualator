using System;
using Autofac;
using Caliburn.Micro;
using Common;
using RoadTrafficConstructor.Presenters.BuildMode;
using RoadTrafficConstructor.Presenters.SimulationMode;
using RoadTrafficSimulator.Road;
using RoadTrafficSimulator.RoadTrafficSimulatorMessages;
using IEventAggregator = Common.IEventAggregator;

namespace RoadTrafficConstructor.Presenters
{
    public sealed class ShellViewModel : Conductor<object>, IShellViewModel, Common.IHandle<XnaWindowInitialized>
    {
        private readonly Func<IMouseInformationModel> _mouseInformationFactory;
        private readonly IEventAggregator _eventAggreator;
        private readonly ILifetimeScope _container;
        private WorldController _wordController;

        private IMouseInformationModel _mouseInformation;
        private bool _isBuildMode;

        public ShellViewModel(
            Func<IMouseInformationModel> mouseInformationFactory,
            IEventAggregator eventAggreator,
            ILifetimeScope container )
        {
            this._container = container;
            this._eventAggreator = eventAggreator;
            this._mouseInformationFactory = mouseInformationFactory;

            this._eventAggreator.Subscribe( this );
            this.ServiceProvider = this._container.Resolve<IServiceProvider>();

            this.SwitchToBuildMode();
        }

        private IServiceProvider ServiceProvider { get; set; }

        public void Initialize( WorldController worldController )
        {
            this.MouseInformationModel = this._mouseInformationFactory();
            this._wordController = worldController.NotNull();
        }

        public void IncreaseZoom()
        {
            if ( this._wordController == null ) { return; }

            var zoomValue = this._wordController.GetZoom();
            this._wordController.SetZoom( zoomValue + 0.1f );
        }

        public void DecreaseZoom()
        {
            if ( this._wordController == null ) { return; }

            var zoomValue = this._wordController.GetZoom();
            this._wordController.SetZoom( zoomValue - 0.1f );
        }

        public IMouseInformationModel MouseInformationModel
        {
            get { return this._mouseInformation; }
            set
            {
                this._mouseInformation = value;
                this.RaisePropertyChangedEventImmediately( "MouseInformationModel" );
            }
        }

        public void Handle( XnaWindowInitialized message )
        {
            this._wordController = this._container.Resolve<WorldController>();
        }

        public bool IsBuildMode
        {
            get { return this._isBuildMode; }
            set
            {
                if ( this._isBuildMode == value ) { return; }
                this._isBuildMode = value;
                this.NotifyOfPropertyChange( () => this.IsBuildMode );
            }
        }

        public void SwtichToSimulationMode()
        {
            this.IsBuildMode = false;
            this.ActivateItem( this._container.Resolve<SimulationViewModel>() );
        }

        public void SwitchToBuildMode()
        {
            this.ActivateItem( this._container.Resolve<BuildJunctionViewModel>() );
            this.IsBuildMode = true;
        }
    }
}
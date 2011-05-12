using System;
using Autofac;
using Caliburn.Micro;
using Common;
using RoadTrafficConstructor.Presenters.BuildMode;
using RoadTrafficConstructor.Presenters.SimulationMode;
using RoadTrafficSimulator.Infrastructure.Messages;
using IEventAggregator = Common.IEventAggregator;

namespace RoadTrafficConstructor.Presenters
{
    public sealed class ShellViewModel : Conductor<object>, IShellViewModel
    {
        private readonly Func<IMouseInformationModel> _mouseInformationFactory;
        private readonly IEventAggregator _eventAggreator;
        private readonly ILifetimeScope _container;

        private IMouseInformationModel _mouseInformation;
        private bool _isBuildMode;
        private int _zoomValue;

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
            this.ZoomValue = 50;
        }

        private IServiceProvider ServiceProvider { get; set; }

        public void Initialize()
        {
            this.MouseInformationModel = this._mouseInformationFactory();
        }

        public void IncreaseZoom()
        {
            this.ZoomValue += 10;
        }

        public void DecreaseZoom()
        {
            this.ZoomValue -= 10;
        }

        public int ZoomValue
        {
            get { return this._zoomValue; }
            set
            {
                var newValue = value;
                if ( newValue > 100 ) { newValue = 100; }
                if ( newValue < 0 ) { newValue = 0; }
                if ( newValue == this._zoomValue ) { return; }
                this._zoomValue = newValue;
                this.NotifyOfPropertyChange( () => this.ZoomValue );
                this._eventAggreator.Publish( new ChangedZoom( newValue ) );
            }
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
            this._eventAggreator.Publish( new ChangedToSimulationMode() );
            this.ActivateItem( this._container.Resolve<SimulationViewModel>() );
        }

        public void SwitchToBuildMode()
        {
            this.IsBuildMode = true;
            this._eventAggreator.Publish( new ChangedToBuildMode() );
            this.ActivateItem( this._container.Resolve<BuildJunctionViewModel>() );
        }
    }
}
using Autofac;
using Caliburn.Micro;
using RoadTrafficConstructor.Presenters.BottomTabs;
using RoadTrafficConstructor.Presenters.BuildMode;
using RoadTrafficConstructor.Presenters.SimulationMode;
using RoadTrafficSimulator.Infrastructure.Messages;
using IEventAggregator = Common.IEventAggregator;

namespace RoadTrafficConstructor.Presenters
{
    public sealed class ShellViewModel : Conductor<object>, IShellViewModel, Common.IHandle<ChangedToBuildMode>, Common.IHandle<ChangedToSimulationMode>
    {
        private readonly ILifetimeScope _container;

        public ShellViewModel( ILifetimeScope container, ControlViewModel controlViewModel, SettingViewModel settings )
        {
            this._container = container;
            this._controlViewModel = controlViewModel;
            this._settings = settings;
            this.ActivateItem( this._container.Resolve<BuildJunctionViewModel>() );
        }

        private readonly ControlViewModel _controlViewModel;
        public ControlViewModel Control { get { return this._controlViewModel; } }

        private readonly SettingViewModel _settings;
        public SettingViewModel Settings { get { return this._settings; } }

        public void Handle( ChangedToBuildMode message )
        {
            this.ActivateItem( this._container.Resolve<BuildJunctionViewModel>() );
        }

        public void Handle( ChangedToSimulationMode message )
        {
            var control = this._container.Resolve<SimulationViewModel>();
            this.ActivateItem( control );
        }
    }
}
using System.Collections.Generic;
using Autofac.Features.Metadata;
using Caliburn.Micro;
using RoadTrafficConstructor.Presenters.BottomTabs.Settings;
using RoadTrafficSimulator.Infrastructure;
using System.Linq;
using RoadTrafficSimulator.Infrastructure.Messages;

namespace RoadTrafficConstructor.Presenters.BottomTabs
{
    public sealed class SettingViewModel : Conductor<ISettingViewMode>, Common.IHandle<ShowSettings>
    {
        private readonly IEnumerable<ISettingViewMode> _settings;

        public SettingViewModel( IEnumerable<Meta<ISettingViewMode, NumberMeta>> settings )
        {
            this._settings = settings.OrderBy( s => s.Metadata.Order ).Select( s => s.Value ).ToArray();
            this.ActivateItem( this._settings.Last() );
        }

        public void Handle( ShowSettings message )
        {
            var viewModel = this._settings.First( s => s.SupportSetting( message.Control ) );
            viewModel.SetStting( message.Control );
            this.ActivateItem( viewModel );
        }
    }
}
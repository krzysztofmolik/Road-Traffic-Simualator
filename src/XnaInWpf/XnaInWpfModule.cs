using Autofac;
using RoadTrafficConstructor.Presenters;
using RoadTrafficConstructor.Properties;
using RoadTrafficSimulator;
using RoadTrafficSimulator.Infrastructure.Mouse;
using EventAggregator = Common.EventAggregator;
using IEventAggregator = Common.IEventAggregator;

namespace RoadTrafficConstructor
{
    public class XnaInWpfModule : Module
    {
        protected override void Load( ContainerBuilder builder )
        {
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterModule( new GameModule( Settings.Default.UseHiDef ) );

            builder.Register( s => new DebugMouseInformationModel( s.ResolveNamed<IMouseInformation>( "MainMouseInformation" ) ) )
                .As<IMouseInformationModel>();
        }
    }
}
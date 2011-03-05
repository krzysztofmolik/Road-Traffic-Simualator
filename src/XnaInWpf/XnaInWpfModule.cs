using System.Linq;
using Autofac;
using Caliburn.Micro;
using RoadTrafficConstructor.Presenters;
using RoadTrafficConstructor.Presenters.Blocks;
using RoadTrafficSimulator;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Utile.DependencyInjection;
using RoadTrafficSimulator.Utils.DependencyInjection;
using XnaInWpf.Presenters.Blocks;
using EventAggregator = Common.EventAggregator;
using IEventAggregator = Common.IEventAggregator;

namespace RoadTrafficConstructor
{
    public class XnaInWpfModule : Module
    {
        protected override void Load( ContainerBuilder builder )
        {
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterModule( new XnaCustomModule() );
            builder.RegisterModule( new GameModule() );

            builder.RegisterType<BlockManager>().As<IBlockManager>();
            var iBlockNamespace = typeof( IBlockViewModel ).Namespace;
            builder.RegisterAssemblyTypes( typeof( IBlockViewModel ).Assembly ).Where(
                t => t.Namespace == iBlockNamespace && t.GetInterfaces().Contains( typeof( IBlockViewModel ) ) )
                .As( t => typeof( IBlockViewModel ) );

            builder.Register(s => new DebugMouseInformationModel(s.ResolveNamed<IMouseInformation>("MainMouseInformation")))
                .As<IMouseInformationModel>();
        }
    }
}
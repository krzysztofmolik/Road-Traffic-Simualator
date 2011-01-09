using System.Linq;
using Autofac;
using RoadTrafficConstructor.Presenters;
using RoadTrafficConstructor.Presenters.Blocks;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Utile.DependencyInjection;
using WinFormsGraphicsDevice;
using XnaInWpf.Presenters.Blocks;

namespace RoadTrafficConstructor
{
    public class XnaInWpfModule : Module
    {
        protected override void Load( ContainerBuilder builder )
        {
            builder.RegisterModule( new XnaWinFormModule() );
            builder.RegisterModule( new XnaCustomModule() );

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
using Autofac;
using RoadTrafficConstructor.Presenters;
using RoadTrafficConstructor.Presenters.Blocks;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Utils.DependencyInjection;
using WinFormsGraphicsDevice;
using XnaInWpf.Presenters.Blocks;
using System.Linq;
using XnaRoadTrafficConstructor.Infrastucure.Mouse;
using XnaRoadTrafficConstructor.Utils.DependencyInjection;

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
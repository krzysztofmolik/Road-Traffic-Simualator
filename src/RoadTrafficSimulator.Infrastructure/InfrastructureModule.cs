using Autofac;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.DependencyInjection;
using RoadTrafficSimulator.Infrastructure.Draw;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Infrastructure.Textures;
using Game = Arcane.Xna.Presentation.Game;
using GraphicsDeviceManager = Arcane.Xna.Presentation.GraphicsDeviceManager;

namespace RoadTrafficSimulator.Infrastructure
{
    public class InfrastructureModule : Module
    {
        private readonly bool _useHiDef;

        public InfrastructureModule( bool useHiDef )
        {
            _useHiDef = useHiDef;
        }

        protected override void Load( ContainerBuilder builder )
        {
            builder.RegisterType<Camera3D>().SingleInstance();

            builder.RegisterType<ContentManagerAdapter>().AsSelf().As<IContentManagerAdapter>().SingleInstance();
            builder.RegisterType<KeyboardInputNotify>().As<KeyboardInputNotify>().SingleInstance();
            builder.RegisterType<MouseInformation>().As<MouseInformation>().Named<IMouseInformation>( "MainMouseInformation" ).SingleInstance();

            builder.RegisterType<FilterMouseInformation>().As<IMouseInformation>()
                .InstancePerDependency();
            builder.Register(
                s => new PriorityMouseInfomrmation( s.ResolveNamed<IMouseInformation>( "MainMouseInformation" ) ) )
                .InstancePerLifetimeScope();

            builder.RegisterType<SelectedControls>().SingleInstance();
            builder.RegisterType<MoveControl>().SingleInstance();
            builder.RegisterType<NotMovableMouseHandler>().As<IMouseHandler>().InstancePerDependency();
            builder.RegisterType<Graphic>().SingleInstance();
            builder.RegisterType<VertexPositionColorDrawer>();
            builder.RegisterType<VertexPositionTextureDrawer>();
            builder.Register( c => new GraphicsDeviceManager( c.Resolve<Game>() ) ).As<IGraphicsDeviceManager, IGraphicsDeviceService>().SingleInstance();
            builder.RegisterType<TextureManager>().SingleInstance();
            builder.Register( s => s.Resolve<IGraphicsDeviceService>().GraphicsDevice ).InstancePerDependency();
            builder.Register( s => s.Resolve<Game>().Content ).SingleInstance();
        }
    }
}
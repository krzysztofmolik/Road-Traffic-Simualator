using Autofac;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.DependencyInjection;
using RoadTrafficSimulator.Infrastructure.Draw;
using RoadTrafficSimulator.Infrastructure.Mouse;
using Game = Arcane.Xna.Presentation.Game;
using GraphicsDeviceManager = Arcane.Xna.Presentation.GraphicsDeviceManager;

namespace RoadTrafficSimulator.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load( ContainerBuilder builder )
        {
            builder.RegisterType<Camera3D>().SingleInstance();

            builder.RegisterType<ContentManagerAdapter>().As<IContentManager>();
            builder.RegisterType<KeyboardInputNotify>().As<KeyboardInputNotify>().SingleInstance();
            builder.RegisterType<MouseInformation>().As<MouseInformation>().Named<IMouseInformation>("MainMouseInformation").SingleInstance();

            builder.RegisterType<FilterMouseInformation>().As<IMouseInformation>()
                .InstancePerDependency();
            builder.Register(
                s => new PriorityMouseInfomrmation(s.ResolveNamed<IMouseInformation>("MainMouseInformation")))
                .InstancePerLifetimeScope();

            builder.RegisterType<SelectedControls>().SingleInstance();
            builder.RegisterType<MoveControl>().SingleInstance();
            builder.RegisterType<NotMovableMouseHandler>().As<IMouseHandler>().InstancePerDependency();
            builder.RegisterType<Graphic>().SingleInstance();
            builder.RegisterType<VertexPositionColorDrawer>();
            builder.RegisterType<VertexPositionTextureDrawer>();
            builder.RegisterType<GraphicsDeviceManager>().As<IGraphicsDeviceManager, IGraphicsDeviceService>().SingleInstance();
            builder.Register(s => s.Resolve<IGraphicsDeviceService>().GraphicsDevice).InstancePerDependency();
            builder.Register(s => s.Resolve<Game>().Content).InstancePerDependency();
        }
    }
}
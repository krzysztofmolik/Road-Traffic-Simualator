using Autofac;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.Draw;
using Game = Arcane.Xna.Presentation.Game;
using GraphicsDeviceManager = Arcane.Xna.Presentation.GraphicsDeviceManager;

namespace RoadTrafficSimulator.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load( ContainerBuilder builder )
        {
            builder.RegisterType<Graphic>().SingleInstance();
            builder.RegisterType<VertexPositionColorDrawer>();
            builder.RegisterType<VertexPositionTextureDrawer>();
            builder.RegisterType<GraphicsDeviceManager>().As<IGraphicsDeviceManager, IGraphicsDeviceService>().SingleInstance();
            builder.Register(s => s.Resolve<IGraphicsDeviceService>().GraphicsDevice).InstancePerDependency();
            builder.Register(s => s.Resolve<Game>().Content).InstancePerDependency();
        }
    }
}
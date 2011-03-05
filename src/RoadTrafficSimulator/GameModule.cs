using Autofac;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game = Arcane.Xna.Presentation.Game;
using GraphicsDeviceManager = Arcane.Xna.Presentation.GraphicsDeviceManager;

namespace RoadTrafficSimulator
{
    public class GameModule : Module
    {
        protected override void Load( ContainerBuilder builder )
        {
            builder.RegisterType<XnaWindow>().As<Game, XnaWindow>().SingleInstance();
            builder.RegisterType<GraphicsDeviceManager>().As<IGraphicsDeviceManager, IGraphicsDeviceService>().SingleInstance();
            builder.Register(s => s.Resolve<IGraphicsDeviceService>().GraphicsDevice).InstancePerDependency();
            builder.Register(s => s.Resolve<Game>().Content).InstancePerDependency();
        }
    }
}
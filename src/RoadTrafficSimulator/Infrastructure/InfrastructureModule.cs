using Autofac;
using RoadTrafficSimulator.Infrastructure.Draw;
using RoadTrafficSimulator.Road;
using XnaRoadTrafficConstructor.Infrastucure.Draw;

namespace RoadTrafficSimulator.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load( ContainerBuilder builder )
        {
            builder.RegisterType<Graphic>().SingleInstance();
            builder.RegisterType<VertexPositionColorDrawer>();
            builder.RegisterType<VertexPositionTextureDrawer>();
        }
    }
}
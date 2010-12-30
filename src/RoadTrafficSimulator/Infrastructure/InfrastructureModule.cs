using Autofac;
using RoadTrafficSimulator.Infrastructure.Draw;
using XnaRoadTrafficConstructor.Infrastucure.Draw;
using XnaRoadTrafficConstructor.Road;

namespace XnaRoadTrafficConstructor.Infrastucure
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
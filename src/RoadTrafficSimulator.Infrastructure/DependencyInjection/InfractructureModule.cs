using Autofac;
using RoadTrafficSimulator.Infrastructure.Mouse;
using Module = Autofac.Module;

namespace RoadTrafficSimulator.Infrastructure.DependencyInjection
{
    public class InfractructureModule : Module
    {
        protected override void Load( ContainerBuilder builder )
        {

            builder.RegisterType<Camera3D>().SingleInstance();

            builder.RegisterType<ContentManagerAdapter>().As<IContentManager>();
            builder.RegisterType<KeyboardInputNotify>().As<KeyboardInputNotify>().SingleInstance();
            builder.RegisterType<MouseInformation>().As<IMouseInformation>().SingleInstance();

            // NOTE Mouse support
            builder.RegisterType<MouseInformation>().Named<IMouseInformation>("MainMouseInformation")
                .InstancePerLifetimeScope()
                .OnActivated(t => t.Instance.StartRecord());
            builder.RegisterType<FilterMouseInformation>().As<IMouseInformation>()
                .InstancePerDependency();
            builder.Register(
                s => new PriorityMouseInfomrmation(s.ResolveNamed<IMouseInformation>("MainMouseInformation")))
                .InstancePerLifetimeScope();

            builder.RegisterType<SelectedControls>().SingleInstance();
            builder.RegisterType<MoveControl>().SingleInstance();
            builder.RegisterType<NotMovableMouseHandler>().As<IMouseHandler>().InstancePerDependency();
        }
    }
}
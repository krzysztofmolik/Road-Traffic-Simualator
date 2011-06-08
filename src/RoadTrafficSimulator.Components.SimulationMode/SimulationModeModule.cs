using System;
using System.Reflection;
using Autofac;
using RoadTrafficSimulator.Components.SimulationMode.CarsSpecification;
using RoadTrafficSimulator.Components.SimulationMode.Controlers;
using Common.Extensions;
using Module = Autofac.Module;

namespace RoadTrafficSimulator.Components.SimulationMode
{
    public class SimulationModeModule : Module
    {
        protected override void Load( ContainerBuilder builder )
        {
            builder.RegisterType<SimulationModeMainComponent>().SingleInstance();
            builder.RegisterType<Builder>().InstancePerLifetimeScope();
            builder.RegisterType<CarsFactory>().As<ICarsFactory>().InstancePerDependency();
            builder.RegisterAssemblyTypes( Assembly.GetExecutingAssembly() )
                       .InNamespaceOf<IControlers>()
                       .Where( t => t.IsImplementingInterface<IControlers>() )
                       .AsImplementedInterfaces()
                       .AsSelf()
                       .SingleInstance();

            this.RegisterCarsSpecifications( builder );
        }

        private void RegisterCarsSpecifications( ContainerBuilder builder )
        {
            builder.RegisterAssemblyTypes( Assembly.GetExecutingAssembly() )
                .InNamespaceOf<ICarSpecifiaction>()
                .Where( t => t.IsImplementingInterface<ICarSpecifiaction>() )
                .As<ICarSpecifiaction>()
                .InstancePerLifetimeScope();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using RoadTrafficSimulator.Components.SimulationMode.Builder;
using RoadTrafficSimulator.Components.SimulationMode.CarsSpecification;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Conductors.Factories;
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
            builder.RegisterType<Builder.Builder>().InstancePerLifetimeScope();
            builder.RegisterType<CarsFactory>().As<ICarsFactory>().InstancePerDependency();
            builder.RegisterAssemblyTypes( Assembly.GetExecutingAssembly() )
                       .InNamespaceOf<IControlers>()
                       .Where( t => t.IsImplementingInterface<IControlers>() )
                       .AsImplementedInterfaces()
                       .AsSelf()
                       .SingleInstance();

            this.RegisterCarsSpecifications( builder );
            this.RegisterConductors( builder );
            this.RegisterConductorFactories( builder );
            this.RegisterRoadElementBuilders( builder );
        }

        private void RegisterConductors( ContainerBuilder builder )
        {
            builder.RegisterAssemblyTypes( typeof( IConductor ).Assembly ).Where( s => s.IsImplementingInterface<IConductor>() ).AsSelf().As<IConductor>().InstancePerDependency();
        }

        private void RegisterRoadElementBuilders( ContainerBuilder builder )
        {
            builder.RegisterAssemblyTypes( typeof( Builder.Builder ).Assembly ).Where( s => s.IsImplementingInterface<IBuilerItem>() ).As<IBuilerItem>().InstancePerDependency();
        }

        private void RegisterConductorFactories( ContainerBuilder builder )
        {
            var composite = typeof( CondutctorFactory );
            builder.RegisterAssemblyTypes( composite.Assembly ).Where( s => s.IsImplementingInterface<IConductorFactory>() && s != composite ).Named<IConductorFactory>( "conductorFactoryImpl" );
            builder.Register( s => new CondutctorFactory( s.ResolveNamed<IEnumerable<IConductorFactory>>( "conductorFactoryImpl" ) ) ) .As<IConductorFactory>();
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
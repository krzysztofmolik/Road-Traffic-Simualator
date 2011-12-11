using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Castle.Core.Internal;
using RoadTrafficSimulator.Components.SimulationMode.Builder;
using RoadTrafficSimulator.Components.SimulationMode.CarsSpecification;
using RoadTrafficSimulator.Components.SimulationMode.Controlers;
using Common.Extensions;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors.Infrastructure;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Factories;
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
            this.RegisterRoadInfromation( builder );
            this.RegisterRoadInformationFactories( builder );
            this.RegisterRoadElementBuilders( builder );
            this.RegisterRoadInfromation( builder );
            this.RegisterRoadInformationFactories( builder );
            this.RegisterConductors( builder );
            this.RegisterFactoryConductors( builder );
        }

        private void RegisterFactoryConductors( ContainerBuilder builder )
        {
            builder.RegisterType<RouteToConductorConverter>();
            builder.RegisterType<ConductorResolver>();
            builder.Register( c => new Func<Type, IConductor>( type => (IConductor) c.ResolveService( new TypedService( type ) ) ) );
        }

        private void RegisterConductors( ContainerBuilder builder )
        {
            var baseType = typeof( IConductor );
            builder.RegisterAssemblyTypes( baseType.Assembly )
                .InNamespace( baseType.Namespace )
                .Where( t => t.HasAttribute<PriorityConductorInformationAttribute>() && t.HasAttribute<ConductorSupportedRoadElementTypeAttribute>() )
                .As<IConductor>()
                .AsSelf()
                .InstancePerDependency();
        }

        private void RegisterRoadInfromation( ContainerBuilder builder )
        {
            builder.RegisterAssemblyTypes( typeof( IRoadInformation ).Assembly ).Where( s => s.IsImplementingInterface<IRoadInformation>() ).AsSelf().As<IRoadInformation>().InstancePerDependency();
        }

        private void RegisterRoadElementBuilders( ContainerBuilder builder )
        {
            builder.RegisterAssemblyTypes( typeof( Builder.Builder ).Assembly ).Where( s => s.IsImplementingInterface<IBuilerItem>() ).As<IBuilerItem>().InstancePerDependency();
        }

        private void RegisterRoadInformationFactories( ContainerBuilder builder )
        {
            var composite = typeof( CondutctorFactory );
            builder.RegisterAssemblyTypes( composite.Assembly ).Where( s => s.IsImplementingInterface<IRoadInformationFactory>() && s != composite ).Named<IRoadInformationFactory>( "conductorFactoryImpl" );
            builder.Register( s => new CondutctorFactory( s.ResolveNamed<IEnumerable<IRoadInformationFactory>>( "conductorFactoryImpl" ) ) ).As<IRoadInformationFactory>();
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
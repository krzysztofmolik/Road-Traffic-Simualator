using System;
using System.Reflection;
using Autofac;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.BuildMode.Commands;
using RoadTrafficSimulator.Components.BuildMode.Connectors;
using RoadTrafficSimulator.Components.BuildMode.Connectors.Commands;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.BuildMode.Factories;
using RoadTrafficSimulator.Components.BuildMode.PersiserModel.Commands;
using RoadTrafficSimulator.Components.BuildMode.PersiserModel.Converters;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Mouse;
using Module = Autofac.Module;
using Common.Extensions;

namespace RoadTrafficSimulator.Components.BuildMode
{
    public class BuildModeModule : Module
    {
        protected override void Load( ContainerBuilder builder )
        {
            builder.RegisterType<RoadLayer>().InstancePerLifetimeScope();
            builder.Register( s => new VisitAllChildren( s.Resolve<RoadLayer>() ) ).SingleInstance();
            builder.RegisterType<BuildModeMainComponent>().InstancePerLifetimeScope();
            builder.RegisterType<BuilderCommandManager>().InstancePerLifetimeScope();
            builder.RegisterType<CompositeConnectionCommand>();

            builder.RegisterType<RoadLaneBuilder>();
            builder.RegisterType<ConnectRoadJunctionEdge>().As<IConnectionCommand>();
            builder.RegisterType<ConnectRoadLaneWithRoadConnection>().As<IConnectionCommand>();
            builder.RegisterType<ConnectRoadLaneWithJunctionEdge>().As<IConnectionCommand>();
            builder.RegisterType<ConnectRoadConnectionWithRoadLane>().As<IConnectionCommand>();
            builder.RegisterType<ConnectRoadJunctionEdgeWitEndRoadLaneEdge>().As<IConnectionCommand>();
            builder.RegisterType<ConnectSideRoadLaneEdges>().As<IConnectionCommand>();
            builder.RegisterType<ConnectCarInserterWithEndRoadLane>().As<IConnectionCommand>();
            builder.RegisterType<ConnectCarsInserterWithCarsInserter>().As<IConnectionCommand>();
            builder.RegisterType<ConnectCarsRemoverWithCarsRemover>().As<IConnectionCommand>();
            builder.RegisterType<ConnectRoadLaneWithCarsRemover>().As<IConnectionCommand>();
            builder.RegisterType<ConnectRoadConnectionWithRoadConnection>().As<IConnectionCommand>();
            builder.RegisterType<MouseHandlerFactory>().SingleInstance();
            builder.RegisterType<ControlFactories>().SingleInstance().AsSelf().AsImplementedInterfaces();
            builder.Register( c => new Func<Vector2, JunctionEdge>( location => new JunctionEdge( c.Resolve<Factories.Factories>(), location ) ) );
            builder.Register( c => new Func<Vector2, RoadJunctionBlock>( location => new RoadJunctionBlock( c.Resolve<Factories.Factories>(), location ) ) );

            builder.Register(
                s =>
                new Func<RoadLayer, IMouseHandler>( r => new RoadLayerMouseHandler( r, s.Resolve<SelectedControls>() ) ) );
            builder.Register(
                s =>
                new Func<IControl, IMouseHandler>(
                    r => new SingleControlMouseHandler( r, s.Resolve<SelectedControls>(), s.Resolve<MoveControl>() ) ) );
            builder.Register(
                s =>
                new Func<ICompositeControl, IMouseHandler>(
                    r => new CompositeControlMouseHandler( r, s.Resolve<SelectedControls>(), s.Resolve<MoveControl>() ) ) );
            this.RegisterFactoryMethods( builder );

            this.RegisterFactories( builder );
            this.RegisterCreators( builder );

            builder.RegisterType<ControlSerializer>().InstancePerLifetimeScope();
            this.RegisterConvtrolConverters( builder );

            builder.RegisterType<DeserializationContext>();
        }

        private void RegisterConvtrolConverters( ContainerBuilder builder )
        {
            builder.RegisterAssemblyTypes( Assembly.GetAssembly( typeof( IControlConverter ) ) )
                .Where( s => s.Namespace == typeof( IControlConverter ).Namespace )
                .Where( s => s.IsImplementingInterface<IControlConverter>() )
                .As<IControlConverter>();
        }

        private void RegisterCreators( ContainerBuilder builder )
        {
            builder.RegisterAssemblyTypes( Assembly.GetAssembly( typeof( ICommand ) ) )
                .Where( s => s.IsImplementingInterface<ICommand>() ).As<ICommand>();
        }

        private void RegisterFactoryMethods( ContainerBuilder builder )
        {
            builder.Register( s => new Func<Vector2, ICompositeControl>( location => new RoadJunctionBlock( s.Resolve<Factories.Factories>(), location ) ) );

            builder.Register( s => new Func<RoadLaneBlock>( () => new RoadLaneBlock( s.Resolve<Factories.Factories>()) ) );
            builder.Register( s => new Func<Vector2,  RoadConnection>( locatio => new RoadConnection( s.Resolve<Factories.Factories>(), locatio ) ) );
        }

        private void RegisterFactories( ContainerBuilder builder )
        {
            builder.RegisterType<Factories.Factories>();
            builder.RegisterAssemblyTypes( Assembly.GetAssembly( typeof( Factories.Factories ) ) )
                .Where( s => s.Namespace == typeof( Factories.Factories ).Namespace && s.Name.EndsWith( "Factory" ) )
                .AsImplementedInterfaces();
        }
    }
}
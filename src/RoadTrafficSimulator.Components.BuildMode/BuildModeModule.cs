using System;
using System.Reflection;
using Autofac;
using Common;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.BuildMode.Commands;
using RoadTrafficSimulator.Components.BuildMode.Connectors;
using RoadTrafficSimulator.Components.BuildMode.Connectors.Commands;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Components.BuildMode.Factories;
using RoadTrafficSimulator.Infrastructure.Controls;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Road;
using Module = Autofac.Module;
using Common.Extensions;

namespace RoadTrafficSimulator.Components.BuildMode
{
    public class BuildModeModule : Module
    {
        protected override void Load( ContainerBuilder builder )
        {
            builder.RegisterType<RoadLayer>().InstancePerLifetimeScope();
            builder.Register( s => new VisitAllChildren( s.Resolve<RoadLayer>() ) );
            builder.RegisterType<BuildModeMainComponent>().InstancePerLifetimeScope();
            builder.RegisterType<BuilderCommandManager>().InstancePerLifetimeScope();
            builder.RegisterType<CompositeConnectionCommand>();

            builder.RegisterType<RoadLaneBuilder>();
            builder.RegisterType<ConnectRoadJunctionEdge>().As<IConnectionCommand>();
            builder.RegisterType<ConnectEndRoadLaneEdgeWithRoadConnection>().As<IConnectionCommand>();
            builder.RegisterType<ConnectEndRoadLaneEdgeWithRoadJunctionEdge>().As<IConnectionCommand>();
            builder.RegisterType<ConnectRoadConnectionWithEndRoadLane>().As<IConnectionCommand>();
            builder.RegisterType<ConnectRoadJunctionEdgeWitEndRoadLaneEdge>().As<IConnectionCommand>();
            builder.RegisterType<ConnectSideRoadLaneEdges>().As<IConnectionCommand>();
            builder.RegisterType<ConnectCarInserterWithEndRoadLane>().As<IConnectionCommand>();
            builder.RegisterType<ConnectCarsInserterWithCarsInserter>().As<IConnectionCommand>();
            builder.RegisterType<ConnectCarsRemoverWithCarsRemover>().As<IConnectionCommand>();
            builder.RegisterType<ConnectEndRoadLaneWithCarsRemover>().As<IConnectionCommand>();
            builder.RegisterType<ConnectRoadConnectionWithRoadConnection>().As<IConnectionCommand>();
            builder.RegisterType<MouseHandlerFactory>().SingleInstance();
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
        }

        private void RegisterCreators( ContainerBuilder builder )
        {
            builder.RegisterAssemblyTypes( Assembly.GetAssembly( typeof( ICommand ) ) )
                .Where( s => s.IsImplementingInterface<ICommand>() ).As<ICommand>();
        }

        private void RegisterFactoryMethods( ContainerBuilder builder )
        {
            builder.Register( s => new Func<Vector2, ICompositeControl, IRoadJunctionBlock>(
                                      ( location, owner ) =>
                                      new RoadJunctionBlock( s.Resolve<Factories.Factories>(), location, owner ) ) );

            builder.Register( s => new Func<ICompositeControl, IRoadLaneBlock>(
                                      cc => new RoadLaneBlock( s.Resolve<Factories.Factories>(), cc ) ) );
            builder.Register( s => new Func<Vector2, ICompositeControl, RoadConnection>(
                                      ( locatio, owner ) =>
                                      new RoadConnection( s.Resolve<Factories.Factories>(), locatio, owner ) ) );
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
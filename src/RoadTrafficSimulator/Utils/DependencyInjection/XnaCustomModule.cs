using System;
using System.Reflection;
using Autofac;
using Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Factories;
using RoadTrafficSimulator.Infrastructure;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Integration;
using RoadTrafficSimulator.Road;
using RoadTrafficSimulator.Road.Connectors.Commands;
using RoadTrafficSimulator.Road.Controls;
using RoadTrafficSimulator.Utile.DependencyInjection;
using Xna;
using XnaRoadTrafficConstructor.MouseHandler.JunctionMouseHandler;
using XnaRoadTrafficConstructor.Utils.DependencyInjection;
using XnaVs10.Sprites;
using Module = Autofac.Module;

namespace RoadTrafficSimulator.Utils.DependencyInjection
{
    public class XnaCustomModule : Module
    {
        protected override void Load( ContainerBuilder builder )
        {
            builder.RegisterType<MessageBroker>().SingleInstance();

            builder.RegisterType<Camera3D>().SingleInstance();
            builder.RegisterType<SpriteBatch>().As<SpriteBatch>();
            builder.RegisterType<PrimitiveBatch>().As<PrimitiveBatch>();
            builder.RegisterType<RoadLayer>().InstancePerLifetimeScope();
            builder.Register( s => new VisitAllChildren( s.Resolve<RoadLayer>() ) );


            builder.RegisterType<ContentManagerAdapter>().As<IContentManager>();
            builder.RegisterType<KeyboardInputNotify>().As<KeyboardInputNotify>().SingleInstance();
            builder.RegisterType<MouseInputNotify>().As<MouseInputNotify>().SingleInstance();
            builder.RegisterType<RoadComponent>()
                .OnActivated( s =>
                                  {
                                      s.Context.Resolve<BuilderControl>().RoadComponent = s.Instance;
                                      s.Context.Resolve<WorldController>().RoadComponent = s.Instance;
                                      s.Context.Resolve<IEventAggregator>().Subscribe( s.Instance );
                                  } )
                .InstancePerLifetimeScope();
            builder.RegisterType<BuilderControl>().InstancePerLifetimeScope();
            builder.RegisterType<WorldController>().InstancePerLifetimeScope();
            builder.RegisterType<Layer2D>().SingleInstance();

            // NOTE Mouse support
            builder.RegisterType<MouseInformation>().Named<IMouseInformation>( "MainMouseInformation" )
                                                     .InstancePerLifetimeScope()
                                                     .OnActivated( t => t.Instance.StartRecord() );
            builder.RegisterType<FilterMouseInformation>().As<IMouseInformation>()
                                                          .InstancePerDependency();
            builder.Register( s => new PriorityMouseInfomrmation( s.ResolveNamed<IMouseInformation>( "MainMouseInformation" ) ) )
                   .InstancePerLifetimeScope();

            builder.RegisterType<RoadLaneCreator>();
            builder.RegisterType<RoadLaneCreatorController>();
            builder.RegisterType<RoadJunctionCreator>();
            builder.RegisterType<CarsInserterCreator>();
            builder.RegisterType<CarsRemoverCreator>();

            builder.RegisterType<JunctionCornerMouseHandler>();
            builder.RegisterType<JunctionEdgeMouseHandler>();

            builder.RegisterModule( new InfrastructureModule() );

            builder.RegisterType<ConnectObjectCommand>();
            builder.RegisterType<CompositeConnectionCommand>();
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
            builder.RegisterType<ScreenZoom>().As<IBackgroundJob>();

            builder.RegisterType<SelectedControls>().SingleInstance();
            builder.RegisterType<MoveControl>().SingleInstance();
            builder.RegisterType<MouseHandlerFactory>().SingleInstance();
            builder.Register( s => new Func<RoadLayer, IMouseHandler>( r => new RoadLayerMouseHandler( r, s.Resolve<SelectedControls>() ) ) );
            builder.Register( s => new Func<IControl, IMouseHandler>( r => new SingleControlMouseHandler( r, s.Resolve<SelectedControls>(), s.Resolve<MoveControl>() ) ) );
            builder.Register( s => new Func<ICompositeControl, IMouseHandler>( r => new CompositeControlMouseHandler( r, s.Resolve<SelectedControls>(), s.Resolve<MoveControl>() ) ) );
            builder.RegisterType<NotMovableMouseHandler>().As<IMouseHandler>().InstancePerDependency();


            this.RegisterFactoryMethods( builder );

            this.RegisterFactories( builder );
        }

        private void RegisterFactoryMethods( ContainerBuilder builder )
        {
            builder.Register( s => new Func<Vector2, ICompositeControl, IRoadJunctionBlock>(
                                       ( location, owner ) => new RoadJunctionBlock( s.Resolve<Factories.Factories>(), location, owner ) ) );

            builder.Register( s => new Func<ICompositeControl, IRoadLaneBlock>(
                                      cc => new RoadLaneBlock( s.Resolve<Factories.Factories>(), cc ) ) );
            builder.Register( s => new Func<Vector2, ICompositeControl, RoadConnection>(
                                      ( locatio, owner ) => new RoadConnection( s.Resolve<Factories.Factories>(), locatio, owner ) ) );
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
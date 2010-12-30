using Autofac;
using Microsoft.Xna.Framework.Graphics;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Integration;
using RoadTrafficSimulator.Road;
using RoadTrafficSimulator.Road.RoadJoiners;
using Xna;
using XnaRoadTrafficConstructor;
using XnaRoadTrafficConstructor.Infrastucure;
using XnaRoadTrafficConstructor.Infrastucure.Mouse;
using XnaRoadTrafficConstructor.MouseHandler;
using XnaRoadTrafficConstructor.MouseHandler.JunctionMouseHandler;
using XnaRoadTrafficConstructor.Road;
using XnaRoadTrafficConstructor.Utils.DependencyInjection;
using XnaVs10.Road;
using XnaVs10.Sprites;
using XnaVs10.Utils;

namespace RoadTrafficSimulator.Utils.DependencyInjection
{
    public class XnaCustomModule : Module
    {
        protected override void Load( ContainerBuilder builder )
        {
            builder.RegisterType<MessageBroker>().SingleInstance();
            builder.RegisterType<XnaWindow>().As<XnaWindow>().SingleInstance();

            builder.RegisterType<Camera3D>().SingleInstance();
            builder.RegisterType<SpriteBatch>().As<SpriteBatch>();
            builder.RegisterType<PrimitiveBatch>().As<PrimitiveBatch>();
            builder.RegisterType<Stored>().SingleInstance();
            builder.RegisterType<RoadLayer>().InstancePerLifetimeScope();
            builder.Register( s => new VisitAllChildren( s.Resolve<RoadLayer>() ) );


            builder.RegisterType<ContentManagerAdapter>().As<IContentManager>();
            builder.RegisterType<KeyboardInputNotify>().As<KeyboardInputNotify>().SingleInstance();
            builder.RegisterType<MouseInputNotify>().As<MouseInputNotify>().SingleInstance();
            builder.RegisterType<RoadComponent>()
                .OnActivated( s => s.Context.Resolve<BuilderControl>().RoadComponent = s.Instance )
                .InstancePerLifetimeScope();
            builder.RegisterType<BuilderControl>().InstancePerLifetimeScope();
            builder.RegisterType<Layer2D>().SingleInstance();
            builder.RegisterType<ControlManager>().As<IControlManager>().SingleInstance();

            //NOTE Mouse support
            builder.RegisterType<MouseInformation>().Named<IMouseInformation>( "MainMouseInformation" )
                                                     .InstancePerLifetimeScope()
                                                     .OnActivated( t => t.Instance.StartRecord() );
            builder.RegisterType<FilterMouseInformation>().As<IMouseInformation>()
                                                          .InstancePerDependency();
            builder.Register( s => new PriorityMouseInfomrmation( s.ResolveNamed<IMouseInformation>( "MainMouseInformation" ) ) )
                   .InstancePerLifetimeScope();

            builder.RegisterType<LineDrawer2D>().SingleInstance();

            builder.RegisterType<RoadLaneCreator>();
            builder.RegisterType<RoadJunctionCreator>();

            builder.RegisterType<JunctionCornerMouseHandler>();
            builder.RegisterType<JunctionEdgeMouseHandler>();

            builder.RegisterType<JuntionMouseHandlerComposite>()
                .As<IMouseHandler>()
                .WithMetadata<IOrderMeta>( order => order.For( s => s.Order, 10 ) );

            builder.RegisterType<MouseDownCompositeHandler>();

            builder.RegisterModule( new InfrastructureModule() );

            builder.RegisterType<ConnectObjectCommand>();
            builder.RegisterType<CompositeConnectionCommand>();
            builder.RegisterType<ConnectEdgeByMoveJunction>().As<IConnectionCommand>();
            builder.RegisterType<ConnectRoadLaneSideEdge>().As<IConnectionCommand>();
            builder.RegisterType<ConnectRoadLaneConnection>().As<IConnectionCommand>();

        }
    }

}
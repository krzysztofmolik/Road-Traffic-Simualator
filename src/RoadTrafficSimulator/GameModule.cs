using Autofac;
using RoadTrafficSimulator.Components.BuildMode;
using RoadTrafficSimulator.Components.SimulationMode;
using RoadTrafficSimulator.Infrastructure;
using Game = Arcane.Xna.Presentation.Game;

namespace RoadTrafficSimulator
{
    public class GameModule : Module
    {
        private readonly bool _useHiDef;

        public GameModule( bool useHiDef )
        {
            this._useHiDef = useHiDef;
        }

        protected override void Load( ContainerBuilder builder )
        {
            builder.RegisterType<XnaWindow>().As<Game, XnaWindow>().SingleInstance();
            builder.RegisterModule( new SimulationModeModule() );
            builder.RegisterModule( new BuildModeModule() );
            builder.RegisterModule( new InfrastructureModule(this._useHiDef) );
        }
    }
}
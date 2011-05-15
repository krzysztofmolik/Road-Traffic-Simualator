using Autofac;

namespace RoadTrafficSimulator.Components.SimulationMode
{
    public class SimulationModeModule : Module
    {
        protected override void Load( ContainerBuilder builder )
        {
            builder.RegisterType<SimulationModeMainComponent>().SingleInstance();
        }
    }
}
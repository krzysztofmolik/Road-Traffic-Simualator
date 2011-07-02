using RoadTrafficSimulator.Components.SimulationMode.Elements.Light;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors.Factories
{
    public class LightConductorFactory : ConductorFactoryBase<Light>
    {
        protected override IConductor Create( Light roadElemnet )
        {
            return null;
        }
    }
}
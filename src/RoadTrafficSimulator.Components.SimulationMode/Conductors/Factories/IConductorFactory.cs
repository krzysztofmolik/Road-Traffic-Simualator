namespace RoadTrafficSimulator.Components.SimulationMode.Conductors.Factories
{
    public interface IConductorFactory
    {
        IConductor Create( IRoadElement roadElement );
        bool CanCreate( IRoadElement roadElement );
    }
}
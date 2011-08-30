namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Factories
{
    public interface IRoadInformationFactory
    {
        IRoadInformation Create( IRoadElement roadElement );
        bool CanCreate( IRoadElement roadElement );
    }
}
namespace RoadTrafficSimulator.Components.SimulationMode.Route
{
    public interface IRouteMark
    {
        void SetLoctionOn( IRoadElement roadElement );
        IRoadElement GetPrevious();
        IRoadElement GetNext();
        IRouteMark MoveNext();
        IRoadElement Current { get; }
    }
}
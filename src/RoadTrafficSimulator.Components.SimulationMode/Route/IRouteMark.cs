namespace RoadTrafficSimulator.Components.SimulationMode.Route
{
    public interface IRouteMark
    {
        void SetLoctionOn( IRoadElement roadElement );
        IRoadElement GetPrevious();
        IRoadElement GetNext();
        void MoveNext();
        IRoadElement Current { get; }
    }
}
namespace RoadTrafficSimulator.Components.SimulationMode.Route
{
    public interface IRouteMark
    {
        void SetLoctionOn( IRoadElement roadElement );
        IRoadElement GetPrevious();
        IRoadElement GetNext();
        bool MoveNext();
        IRoadElement Current { get; }
        IRouteMark MovePrevious();
        IRouteMark Clone();
    }
}
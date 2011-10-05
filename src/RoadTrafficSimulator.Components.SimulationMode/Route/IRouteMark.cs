using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.Route
{
    public interface IRouteMark<T>
    {
        void SetLoctionOn( T roadElement );
        T GetPrevious();
        T GetNext();
        bool MoveNext();
        T Current { get; }
        IRouteMark<T> Clone();
    }
}
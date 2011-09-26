using RoadTrafficSimulator.Components.SimulationMode.Controlers;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.New
{
    public class JunctionConductor : IConductor
    {
        private LaneJucntionInformation _junction;

        public override void OnExit( Car car )
        {
            this._junction.OnExit( car );
        }

        public override void OnEnter( Car car )
        {
            this._junction.OnEnter( car );
        }

        public override bool ShouldChange( Car car )
        {
            return this._junction.ShouldChange( car );
        }

        public override float GetStopPoint(Car car)
        {
        }
    }

    public class RoutePart
    {
    }
}

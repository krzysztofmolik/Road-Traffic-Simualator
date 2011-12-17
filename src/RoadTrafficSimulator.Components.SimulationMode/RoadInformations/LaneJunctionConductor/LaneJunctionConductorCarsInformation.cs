using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.LaneJunctionConductor
{
    public class LaneJunctionConductorCarsInformation
    {
        private readonly CarsQueue _cars = new CarsQueue();
        private readonly LaneJunction _laneJunction;

        public LaneJunctionConductorCarsInformation( LaneJunction laneJunction )
        {
            this._laneJunction = laneJunction;
        }

        public void Take( Car car )
        {
            this._cars.Add( car );
//            if( this._cars.Count > 1 ) { throw new InvalidOperationException();}
        }

        public void Remove( Car car )
        {
            this._cars.Remove( car );
        }
    }
}
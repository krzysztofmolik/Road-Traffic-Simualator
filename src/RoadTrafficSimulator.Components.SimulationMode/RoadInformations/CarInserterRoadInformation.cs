using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations
{
    public class CarInserterRoadInformation : RoadInformationBase, IRoadInformation
    {
        private readonly CarsInserter _carInserter;

        public CarInserterRoadInformation( CarsInserter carInserter )
        {
            Contract.Requires( carInserter != null );
            this._carInserter = carInserter;
        }

        protected override Vector2 GetBeginLocation()
        {
            return this._carInserter.BuildControl.Location;
        }

        public bool ShouldChange( Car car )
        {
            return true;
        }

        protected override Vector2 GetEndLocation()
        {
            return this._carInserter.CarsInserterBuilder.Location;
        }
    }
}

using System;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using RoadTrafficSimulator.Infrastructure;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations
{
    public class LaneCornerRoadInformation : IRoadInformation
    {
        private readonly LaneCorner _laneCorner;
        private readonly CarsQueue _cars = new CarsQueue();

        public LaneCornerRoadInformation( LaneCorner laneCorner )
        {
            Contract.Requires( laneCorner != null );
            this._laneCorner = laneCorner;
        }

        public void OnEnter( Car car )
        {
            Contract.Requires( car != null );
            this._cars.Add( car );
        }

        public void OnExit( Car car )
        {
            this._cars.Remove( car );
        }

        public float Lenght(IRoadElement previous, IRoadElement next)
        {
            return Constans.PointSize;
        }

    }
}
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

        public float Lenght( IRoadElement previous, IRoadElement next )
        {
            return Constans.PointSize;
        }

        public bool ShouldChange( Car car )
        {
            var distance = this._laneCorner.BuildControl.Location - car.Location;
            if ( distance.Length() <= 0.001f ) { return true; }

            return Math.Sign( distance.X ) != Math.Sign( car.Direction.X ) && Math.Sign( distance.Y ) != Math.Sign( car.Direction.Y );
        }

        public void GetCarAheadDistance( IRouteMark<IRoadElement> routMark, CarInformation carInformation )
        {
            throw new NotImplementedException();
        }

        public void GetFirstCarToOutInformation( FirstCarToOutInformation carInformation )
        {
            throw new NotImplementedException();
        }

        public Vector2 GetCarDirection( Car car, IRoadElement nextPoint )
        {
            return _laneCorner.Next.RoadInformation.GetCarDirection( car, nextPoint );
        }

        public float GetCarDistanceTo( Car car, IRoadElement nextPoint )
        {
            throw new NotImplementedException();
        }
    }
}
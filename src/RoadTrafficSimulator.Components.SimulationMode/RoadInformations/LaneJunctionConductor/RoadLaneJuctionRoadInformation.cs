using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.LaneJunctionConductor
{
    public class RoadLaneJuctionRoadInformation : IRoadInformation
    {
        private readonly LaneJunction _laneJunction;
        private readonly CarsQueue _cars = new CarsQueue();
        private readonly List<IRoadElement> _connections = new List<IRoadElement>();
        private readonly List<IRoadElement> _reversConnections = new List<IRoadElement>();

        public RoadLaneJuctionRoadInformation( LaneJunction laneJunction )
        {
            Contract.Requires( laneJunction != null );
            this._laneJunction = laneJunction;
        }

        public void OnEnter( Car car )
        {
            this._cars.Add( car );
        }

        public void OnExit( Car car )
        {
            this._cars.Remove( car );
        }

        public float Lenght( IRoadElement previous, IRoadElement next )
        {
            Debug.Assert( previous is JunctionEdge );
            Debug.Assert( next is JunctionEdge );
            return Vector2.Distance( previous.BuildControl.Location, next.BuildControl.Location );
        }

        public bool ShouldChange( Car car )
        {
            var next = car.Conductors.GetNext().RoadElement;
            var distance = next.BuildControl.Location - car.Location;
            if ( distance.Length() <= 0.001f ) { return true; }

            return Math.Sign( distance.X ) != Math.Sign( car.Direction.X ) && Math.Sign( distance.Y ) != Math.Sign( car.Direction.Y );
        }

        public CarAhedInformation GetCarAheadDistance( Car car )
        {
            if ( this._cars.Contains( car ) )
            {
                var carAhed = this._cars.GetCarAheadOf( car );
                if ( carAhed != null )
                {
                    return new CarAhedInformation
                               {
                                   CarAhead = carAhed,
                                   CarDistance = Vector2.Distance( car.Location, carAhed.Location ),
                               };
                }

                return CarAhedInformation.Empty;
            }

            var firstCar = this._cars.GetFirstCar();
            if ( firstCar != null )
            {
                return new CarAhedInformation
                           {
                               CarAhead = firstCar,
                               CarDistance = 0.0f,
                           };
            }

            return CarAhedInformation.Empty;
        }

        public FirstCarToOutInformation GetFirstCarToOutInformation()
        {
            throw new NotImplementedException();
        }

        public Vector2 GetCarDirection( Car car, IRoadElement nextPoint )
        {
            return nextPoint.BuildControl.Location - car.Location;
        }

        public float GetCarDistanceTo( Car car, IRoadElement nextPoint )
        {
            throw new System.NotImplementedException();
        }

        public void SetConnection( IRoadElement roadElement )
        {
            this._connections.Add( roadElement );
        }

        public void SetReversConnection( IRoadElement roadElement )
        {
            this._reversConnections.Add( roadElement );
        }

        public float GetDistanceToStopLine()
        {
            return float.MaxValue;
        }
    }
}
using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.RoadInformations.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Route;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.LaneJunctionConductor
{
    public class RoadLaneJuctionRoadInformation : IRoadInformation
    {
        private readonly LaneJunction _laneJunction;
        private readonly CarsQueue _cars = new CarsQueue();

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
            var next = car.Conductors.GetNext().RouteElement;
            var distance = next.RoadElement.BuildControl.Location - car.Location;
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

        private IConductor GetNextPoint( IRouteMark<IConductor> conductors )
        {
            var clone = conductors.Clone();
            while ( clone.Current.RouteElement.RoadElement != this._laneJunction )
            {
                clone.MoveNext();
            }

            return clone.GetNext();
        }

        public FirstCarToOutInformation GetFirstCarToOutInformation()
        {
            return FirstCarToOutInformation.Empty;
        }

        public bool ContainsCar( Car car )
        {
            return this._cars.Contains( car );
        }
    }
}
using System.Linq;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Route;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors.LaneJunctionConductor
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
        }

        public void Remove( Car car )
        {
            this._cars.Remove( car );
        }

        public void GetCarAheadDistance( IRouteMark routMark, CarInformation carInformation )
        {
            // TODO Refactory all methos GetCarAheadDistance
            var previousEdge = this.GetEdgeConnectedWith( routMark.GetPrevious() );
            var nextEdge = this.GetEdgeConnectedWith( routMark.GetNext() );
            if ( this._cars.Contains( carInformation.Car ) )
            {
                var carAhead = this._cars.GetCarAheadOf( carInformation.Car );
                if ( carAhead != null )
                {
                    carInformation.CarDistance += Vector2.Distance( carInformation.Car.Location, carAhead.Location );
                    carInformation.CarAhead = carAhead;
                }
                else
                {

                    carInformation.CarDistance += Vector2.Distance( carInformation.Car.Location, nextEdge.EdgeBuilder.Location );
                    routMark.MoveNext();
                    routMark.Current.Condutor.GetCarAheadDistance( routMark, carInformation );
                }
            }
            else
            {
                var cA = this._cars.GetFirstCar();
                if ( cA != null )
                {
                    carInformation.CarDistance += Vector2.Distance( previousEdge.EdgeBuilder.Location, cA.Location );
                    carInformation.CarAhead = cA;
                }
                else
                {
                    carInformation.CarDistance += Vector2.Distance( previousEdge.EdgeBuilder.Location, nextEdge.EdgeBuilder.Location );
                    routMark.MoveNext();
                    routMark.Current.Condutor.GetCarAheadDistance( routMark, carInformation );
                }
            }
        }

        public float GetCarDistanceToEnd( Car car )
        {
            if ( this._cars.Contains( car ) == false ) { return float.MaxValue; }

            var endEdge = this.GetEdgeConnectedWith( car.Route.GetNext() );
            return Vector2.Distance( car.Location, endEdge.EdgeBuilder.Location );
        }

        private JunctionEdge GetEdgeConnectedWith( IRoadElement roadElement )
        {
            var item = this._laneJunction.Edges.Where( s => s.Lane == roadElement ).FirstOrDefault();
            return item;
        }
    }
}
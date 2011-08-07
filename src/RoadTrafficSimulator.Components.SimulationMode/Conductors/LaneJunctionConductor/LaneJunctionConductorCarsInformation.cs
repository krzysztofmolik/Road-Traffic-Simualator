using System.Linq;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using Common;

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
            if ( this._cars.Contains( carInformation.QuestioningCar ) )
            {
                var carAhead = this._cars.GetCarAheadOf( carInformation.QuestioningCar );
                if ( carAhead != null )
                {
                    carInformation.CarDistance += Vector2.Distance( carInformation.QuestioningCar.Location, carAhead.Location );
                    carInformation.CarAhead = carAhead;
                }
                else
                {

                    carInformation.CarDistance += Vector2.Distance( carInformation.QuestioningCar.Location, nextEdge.EdgeBuilder.Location );
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
            var item = this._laneJunction.Edges.Where( s => s.ConnectedEdge == roadElement ).FirstOrDefault();
            return item;
        }

        public void GetFirstCarToOutInformation( FirstCarToOutInformation carInformation )
        {
            var firstCar = this._cars.GetFirstCar();
            if ( firstCar != null )
            {
                carInformation.Add( firstCar, carInformation.CurrentDistance );
                return;
            }

            var outEdges = this._laneJunction.Edges.Where( s => s.Situation.IsOut == false ).Select( s => s );
            foreach ( var junctionEdgeConductor in outEdges )
            {
                var junctionInformation = new FirstCarToOutInformation { CurrentDistance = carInformation.CurrentDistance };
                junctionEdgeConductor.ConnectedEdge.Condutor.GetFirstCarToOutInformation( junctionInformation );

                junctionInformation.Items.ForEach( s => carInformation.Add( s.Car, s.CarDistance ) );
            }
        }
    }
}
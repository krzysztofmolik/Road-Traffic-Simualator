using System.Linq;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Conductors;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using Common;

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
        }

        public void Remove( Car car )
        {
            this._cars.Remove( car );
        }

        public void GetCarAheadDistance( IRouteMark<IRoadElement> routMark, CarInformation carInformation )
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
//                    routMark.Current.RoadInformation.GetCarAheadDistance( routMark, carInformation );
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
//                    routMark.Current.RoadInformation.GetCarAheadDistance( routMark, carInformation );
                }
            }
        }

        public float GetCarDistanceToEnd( Car car )
        {
            if ( this._cars.Contains( car ) == false ) { return float.MaxValue; }

//            var endEdge = this.GetEdgeConnectedWith( car.RoadElements.GetNext() );
//            return Vector2.Distance( car.Location, endEdge.EdgeBuilder.Location );
            return 0.0f;
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

            var outEdges = this._laneJunction.Edges.Where( s => s.ConnectedEdge != null )
                                                   .Where( s => s.Situation.IsOut == false );
            foreach ( var junctionEdgeConductor in outEdges )
            {
                if ( carInformation.VistedElements.Contains( junctionEdgeConductor.ConnectedEdge ) )
                {
                    continue;
                }
                var junctionInformation = new FirstCarToOutInformation( carInformation.VistedElements ) { CurrentDistance = carInformation.CurrentDistance };
                junctionInformation.AddVistedControl( junctionEdgeConductor.ConnectedEdge );
//                junctionEdgeConductor.ConnectedEdge.RoadInformation.GetFirstCarToOutInformation( junctionInformation );

                junctionInformation.Items.ForEach( s => carInformation.Add( s.Car, s.CarDistance ) );
            }
        }
    }
}
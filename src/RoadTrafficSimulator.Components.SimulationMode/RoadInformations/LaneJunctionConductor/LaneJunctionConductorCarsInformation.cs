using System;
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
//            if( this._cars.Count > 1 ) { throw new InvalidOperationException();}
        }

        public void Remove( Car car )
        {
            this._cars.Remove( car );
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
            //            var item = this._laneJunction.Edges.Where( s => s.Next == roadElement ).FirstOrDefault();
            //            return item;
            // Bug
            return null;
        }

        public void GetFirstCarToOutInformation( FirstCarToOutInformation carInformation )
        {
            var firstCar = this._cars.GetFirstCar();
            if ( firstCar != null )
            {
                carInformation.Add( firstCar, carInformation.CurrentDistance );
                return;
            }
            // BUG

            //            var outEdges = this._laneJunction.Edges.Where( s => s.Next != null )
            //                                                   .Where( s => s.Situation.IsOut == false );
            //            foreach ( var junctionEdgeConductor in outEdges )
            //            {
            //                if ( carInformation.VistedElements.Contains( junctionEdgeConductor.Next ) )
            //                {
            //                    continue;
            //                }
            //                var junctionInformation = new FirstCarToOutInformation( carInformation.VistedElements ) { CurrentDistance = carInformation.CurrentDistance };
            //                junctionInformation.AddVistedControl( junctionEdgeConductor.Next );
            //                //                junctionEdgeConductor.ConnectedEdge.RoadInformation.GetFirstCarToOutInformation( junctionInformation );
            //
            //                junctionInformation.Items.ForEach( s => carInformation.Add( s.Car, s.CarDistance ) );
            //            }
        }

        public CarAhedInformation GetCarAheadDistance( Car car )
        {
            var firstCar = this._cars.GetFirstCar();
            if( firstCar != null && firstCar != car)
            {
                return new CarAhedInformation()
                           {
                               CarAhead = firstCar,
                               CarDistance = 0.0f,
                           };
            }

            return CarAhedInformation.Empty;
        }
    }
}
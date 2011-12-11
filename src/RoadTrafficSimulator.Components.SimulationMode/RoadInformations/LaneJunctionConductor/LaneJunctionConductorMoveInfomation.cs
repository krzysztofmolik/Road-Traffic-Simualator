using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.RoadInformations.LaneJunctionConductor
{
    public class LaneJunctionConductorMoveInfomation
    {
        private readonly LaneJunction _laneJunction;

        public LaneJunctionConductorMoveInfomation( LaneJunction laneJunction )
        {
            this._laneJunction = laneJunction;
        }

        public IRoadElement GetNextRandomElement( List<IRoadElement> route, Random rng )
        {
            var edge = this._laneJunction.Edges
                .Where( e => e != null )
                .Where( e => e.Situation.IsOut )
                .Where( e => e.ConnectedEdge != null )
                .Where( e => e != route.Last() )
                .ToArray();
            Debug.Assert( edge.Length != 0 );
            return edge[ rng.Next( 0, edge.Length ) ].ConnectedEdge;
        }

//        public bool ShouldChange( Vector2 acutalCarLocation, Car car )
//        {
//            var next = this._laneJunction.Edges.Where( s => s.ConnectedEdge == car.RoadElements.GetNext() ).FirstOrDefault();
//            var distance = next.EdgeBuilder.Location - acutalCarLocation;
            // TODO Check value and extract some kind of property
//
//            return Math.Sign( distance.X ) != Math.Sign( car.Direction.X ) && Math.Sign( distance.Y ) != Math.Sign( car.Direction.Y );
//        }
//
//        public Vector2 GetCarDirection( Car car )
//        {
//            var edge = this.GetEdgeConnectedWith( car.RoadElements.GetNext() );
//            return edge.EdgeBuilder.Location - car.Location;
//        }
//
//        private JunctionEdge GetEdgeConnectedWith( IRoadElement roadElement )
//        {
//            var item = this._laneJunction.Edges.Where( s => s.ConnectedEdge == roadElement ).FirstOrDefault();
//            return item;
//        }
        public void ShouldChange( Vector2 location )
        {
            throw new NotImplementedException();
        }
    }
}
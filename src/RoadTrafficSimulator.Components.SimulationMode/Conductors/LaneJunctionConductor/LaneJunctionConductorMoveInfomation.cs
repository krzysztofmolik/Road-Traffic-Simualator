using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors.LaneJunctionConductor
{
    public class LaneJunctionConductorMoveInfomation
    {
        private readonly LaneJunction _laneJunction;
        private readonly RightHandRuleLaneJuctionConductor _conductor;

        public LaneJunctionConductorMoveInfomation( LaneJunction laneJunction )
        {
            this._laneJunction = laneJunction;
        }

        public IRoadElement GetNextRandomElement( List<IRoadElement> route )
        {
            var edge = this._laneJunction.Edges.Where( e => e != null )
                .Where( e => e.Lane != null )
                .Where( e => e.Lane.Next != this._laneJunction )
                .Where( e => e != route.Last() )
                .FirstOrDefault();
            Debug.Assert( edge != null );
            return edge.Lane;
        }

        public bool ShouldChange( Vector2 acutalCarLocation, Car car )
        {
            var next = this._laneJunction.Edges.Where( s => s.Lane == car.Route.GetNext() ).FirstOrDefault();
            var distance = next.EdgeBuilder.Location - acutalCarLocation;
            // TODO Check value and extract some kind of property

            return Math.Sign( distance.X ) != Math.Sign( car.Direction.X ) && Math.Sign( distance.Y ) != Math.Sign( car.Direction.Y );
        }

        public Vector2 GetCarDirection( Car car )
        {
            var edge = this.GetEdgeConnectedWith( car.Route.GetNext() );
            return edge.EdgeBuilder.Location - car.Location;
        }

        private JunctionEdge GetEdgeConnectedWith( IRoadElement roadElement )
        {
            var item = this._laneJunction.Edges.Where( s => s.Lane == roadElement ).FirstOrDefault();
            return item;
        }
    }
}
using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.SimulationMode.Elements;
using RoadTrafficSimulator.Components.SimulationMode.Elements.Cars;
using RoadTrafficSimulator.Components.SimulationMode.Route;
using Common;

namespace RoadTrafficSimulator.Components.SimulationMode.Conductors.LaneJunctionConductor
{
    public class LaneJucntionConductorRightHandJunctionInformation
    {
        private readonly LaneJunction _laneJunction;
        public LaneJucntionConductorRightHandJunctionInformation( LaneJunction laneJunction )
        {
            this._laneJunction = laneJunction;
        }

        public void GetNextJunctionInformation( IRouteMark route, JunctionInformation junctionInformation )
        {
            var inEdge = this.GetEdgeConnectedWith( route.GetPrevious() );
            var outEdge = this.GetEdgeConnectedWith( route.GetNext() );

            if ( inEdge.Situation.OnTheRight == outEdge )
            {
                this.TurnRight( inEdge, outEdge, junctionInformation, route );
            }
            else if ( inEdge.Situation.OnTheFront == outEdge )
            {
                this.Straight( inEdge, junctionInformation, route );
            }
            else if ( inEdge.Situation.OnTheLeft == outEdge )
            {
                this.TurnLeft( inEdge, junctionInformation, route );
            }
            else
            {
                throw new InvalidOperationException( "Invalid edge" );
            }
        }

        private void TurnLeft( JunctionEdge inEdge, JunctionInformation junctionInformation, IRouteMark route )
        {
            var onFrontCar = this.GetInformation( route, inEdge.Situation.OnTheFront );
            onFrontCar.Items.ForEach( s => junctionInformation.AddCar( s.Car, s.CarDistance, junctionInformation.JunctionDistance, this.CarTurnLeft( s.Car ) ) );

            var onTheRight = this.GetInformation( route, inEdge.Situation.OnTheRight );
            onTheRight.Items.ForEach( s => junctionInformation.AddCar( s.Car, s.CarDistance, junctionInformation.JunctionDistance ) );

            this.GetInformationFromNext( junctionInformation, route );
        }

        private bool CarTurnLeft( Car car )
        {
            var route = car.Route.Clone();
            while ( route.Current != null && route.Current != this._laneJunction )
            {
                if( !route.MoveNext() )
                {
                    return false;
                }
            }

            if ( route.Current == null ) { return false; }
            var inLane = this.GetEdgeConnectedWith( route.GetPrevious() );
            var outLane = this.GetEdgeConnectedWith( route.GetNext() );
            if ( inLane == null || outLane == null ) { return false; }

            return inLane.Situation.OnTheLeft == outLane;
        }

        private void Straight( JunctionEdge inEdge, JunctionInformation junctionInformation, IRouteMark route )
        {
            var information = this.GetInformation( route, inEdge.Situation.OnTheRight );
            information.Items.ForEach( i => junctionInformation.AddCar( i.Car, i.CarDistance, junctionInformation.JunctionDistance ) );
            this.GetInformationFromNext( junctionInformation, route );
            return;
        }

        private FirstCarToOutInformation GetInformation( IRouteMark route, JunctionEdge junctionEdge )
        {
            var information = new FirstCarToOutInformation( Enumerable.Empty<IRoadElement>() );
            if ( junctionEdge == null || junctionEdge.ConnectedEdge == null || junctionEdge.Situation.IsOut )
            {
                return information;
            }

            junctionEdge.ConnectedEdge.Condutor.GetFirstCarToOutInformation( information );
            return information;
        }

        private void TurnRight( JunctionEdge inEdge, JunctionEdge outEdge, JunctionInformation junctionInformation, IRouteMark route )
        {
            junctionInformation.JunctionDistance += Vector2.Distance( inEdge.EdgeBuilder.Location, outEdge.EdgeBuilder.Location );
            this.GetInformationFromNext( junctionInformation, route );
        }

        private void GetInformationFromNext( JunctionInformation junctionInformation, IRouteMark route )
        {
            route.MoveNext();
            route.Current.Condutor.GetNextJunctionInformation( route, junctionInformation );
        }

        private JunctionEdge GetEdgeConnectedWith( IRoadElement roadElement )
        {
            var item = this._laneJunction.Edges.Where( s => s.ConnectedEdge == roadElement ).FirstOrDefault();
            return item;
        }

        public bool IsPosibleToDriverFrom( IRoadElement roadElement )
        {
            return this._laneJunction.Edges.Where( s => s.Situation.IsOut == false ).Any( s => s.ConnectedEdge == roadElement );
        }

        public bool IsPosibleToDriveTo( IRoadElement roadElement )
        {
            return this._laneJunction.Edges.Where( s => s.Situation.IsOut ).Any( s => s.ConnectedEdge == roadElement );
        }

        public void GetNextAvailablePointToStop( IRouteMark route, NextAvailablePointToStopInfo info )
        {
            var previousEdges = this.GetEdgeConnectedWith( route.GetPrevious() );
            var nextEdge = this.GetEdgeConnectedWith( route.GetNext() );
            info.Length += Vector2.Distance( previousEdges.EdgeBuilder.Location, nextEdge.EdgeBuilder.Location );
            route.MoveNext();
            route.Current.Condutor.GetNextAvailablePointToStop( route, info );
        }
    }
}
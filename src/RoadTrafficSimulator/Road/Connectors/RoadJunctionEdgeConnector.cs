using System;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Road.Controls;
using XnaVs10.Extension;

namespace RoadTrafficSimulator.Road.Connectors
{
    public class RoadJunctionEdgeConnector : ConnectorBase
    {
        private const int MAX_CONNECTED_OBJECT = 1;

        private readonly RoadJunctionEdge _owner;

        public RoadJunctionEdgeConnector( RoadJunctionEdge owner )
            : base( MAX_CONNECTED_OBJECT )
        {
            this._owner = owner;
        }

        public bool AreAllSlotOccupied
        {
            get { return this.CountOfConnectedObject == MAX_CONNECTED_OBJECT; }
        }

        public void ConnectWith( EndRoadLaneEdge roadLaneEdge )
        {
            this.ConnectBySubscribingToEvent( this._owner.StartPoint, roadLaneEdge.EndPoint );
            this.ConnectBySubscribingToEvent( this._owner.EndPoint, roadLaneEdge.StartPoint );
            this.AddConnectedObject( roadLaneEdge );
        }

        public void ConnectTo( EndRoadLaneEdge roadLaneEdge )
        {
            this.ConnectBySubscribingToEvent( this._owner.StartPoint, roadLaneEdge.EndPoint );
            this.ConnectBySubscribingToEvent( this._owner.EndPoint, roadLaneEdge.StartPoint );
            this.AddConnectedObject( roadLaneEdge );
        }

        public void ConnectWith( RoadJunctionEdge roadLaneEdge )
        {
            this.MoveSecondJunction( this._owner, roadLaneEdge );
            this.SubscribeToPointChange( roadLaneEdge );
            this.AddConnectedObject( roadLaneEdge );
        }

        public void ConnectTo( RoadJunctionEdge roadLaneEdge )
        {
            this.SubscribeToPointChange( roadLaneEdge );
            this.AddConnectedObject( roadLaneEdge );
        }

        private void SubscribeToPointChange( RoadJunctionEdge roadLaneEdge )
        {
            this._owner.StartPoint.Changed.Subscribe( s => roadLaneEdge.EndPoint.SetLocation( this._owner.StartPoint.Location ) );
            this._owner.EndPoint.Changed.Subscribe( s => roadLaneEdge.StartPoint.SetLocation( this._owner.EndPoint.Location ) );
        }

        private void MoveSecondJunction( RoadJunctionEdge firstEdge, RoadJunctionEdge secondEdge )
        {
            var secondParent = secondEdge.RoadJunctionParent;

            var firstEdgeCanter = firstEdge.StartLocation + ( ( firstEdge.EndLocation - firstEdge.StartLocation ) / 2 );
            var secondEdgeCenter = secondEdge.StartLocation + ( ( secondEdge.EndLocation - secondEdge.StartLocation ) / 2 );
            var diff = firstEdgeCanter - secondEdgeCenter;

            secondParent.Translate( Matrix.CreateTranslation( diff.ToVector3() ) );
        }
    }
}
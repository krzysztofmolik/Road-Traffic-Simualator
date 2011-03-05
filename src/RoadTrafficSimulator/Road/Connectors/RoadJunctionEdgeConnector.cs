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
        private readonly RoadJunctionEdge _owner;

        public RoadJunctionEdgeConnector( RoadJunctionEdge owner )
        {
            this._owner = owner;
        }

        public Edge PreviousEdge { get; private set; }

        public Edge NextEdge { get; set; }

        public bool AreAllSlotOccupied
        {
            get { return this.PreviousEdge != null && this.NextEdge != null; }
        }

        public void ConnectBeginWith( RoadJunctionEdge roadJunctionEdge )
        {
            roadJunctionEdge.StartPoint.Translated.Subscribe( s => this._owner.EndPoint.SetLocation( s.Control.Location ) );
            roadJunctionEdge.EndPoint.Translated.Subscribe( s => this._owner.StartPoint.SetLocation( s.Control.Location ) );
            this.MoveSecondJunction( roadJunctionEdge, this._owner );
        }

        public void ConnectEndWith( RoadJunctionEdge roadJunctionEdge )
        {
            roadJunctionEdge.StartPoint.Translated.Subscribe( s => this._owner.EndPoint.SetLocation( s.Control.Location ) );
            roadJunctionEdge.EndPoint.Translated.Subscribe( s => this._owner.StartPoint.SetLocation( s.Control.Location ) );
        }

        public void ConnectBeginWith( Edge roadLaneEdge )
        {
            this.ConnectBySubscribingToEvent( this._owner.StartPoint, roadLaneEdge.EndPoint );
            this.ConnectBySubscribingToEvent( this._owner.EndPoint, roadLaneEdge.StartPoint );
            this.PreviousEdge = roadLaneEdge;
        }

        public void ConnectEndWith( Edge edge )
        {
            this.NextEdge = edge;
        }

        // TODO Remove it if not nes...
        private void MoveSecondJunction( RoadJunctionEdge firstEdge, RoadJunctionEdge secondEdge )
        {
            var secondParent = secondEdge.RoadJunctionParent;

            var firstEdgeCanter = firstEdge.StartLocation + ( ( firstEdge.EndLocation - firstEdge.StartLocation ) / 2 );
            var secondEdgeCenter = secondEdge.StartLocation + ( ( secondEdge.EndLocation - secondEdge.StartLocation ) / 2 );
            var diff = firstEdgeCanter - secondEdgeCenter;

            secondParent.Translate( Matrix.CreateTranslation( diff.ToVector3() ) );
        }

        public bool AreConnected( RoadJunctionEdge edge )
        {
            return this.PreviousEdge == edge || this.NextEdge == edge;
        }
    }
}
using System;
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
        }

        public void ConnectEndWith( RoadJunctionEdge roadJunctionEdge )
        {
            roadJunctionEdge.StartPoint.Translated.Subscribe( s => this._owner.EndPoint.SetLocation( s.Control.Location ) );
            roadJunctionEdge.EndPoint.Translated.Subscribe( s => this._owner.StartPoint.SetLocation( s.Control.Location ) );

            var tranlationVector = roadJunctionEdge.Location - this._owner.Location;
            this._owner.Parent.Translate( tranlationVector.ToTranslationMatrix() );
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

        public bool AreConnected( RoadJunctionEdge edge )
        {
            return this.PreviousEdge == edge || this.NextEdge == edge;
        }
    }
}
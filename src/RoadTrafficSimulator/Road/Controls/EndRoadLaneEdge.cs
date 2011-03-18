using System;
using RoadTrafficSimulator.Infrastructure.Control;
using RoadTrafficSimulator.Road.Connectors;

namespace RoadTrafficSimulator.Road.Controls
{
    public class EndRoadLaneEdge : Edge
    {
        private readonly RoadLaneBlock _parrent;

        public EndRoadLaneEdge(Factories.Factories factories,  RoadLaneBlock parent ) 
            : base(factories)
        {
            this._parrent = parent;
            this.Connector = new EndRoadLaneEdgeConnector( this );
        }

        public EndRoadLaneEdge(Factories.Factories factories,  MovablePoint startPoint, MovablePoint endPoint, float width, RoadLaneBlock parent )
            : base( factories, startPoint, endPoint, width )
        {
            this._parrent = parent;
            this.Order = this._parrent.Order + 1;
            this.Connector = new EndRoadLaneEdgeConnector( this );
        }

        public RoadLaneBlock RoadLaneBlockParent
        {
            get { return this._parrent; }
        }

        public override IControl Parent
        {
            get { return this._parrent; }
        }

        public EndRoadLaneEdgeConnector Connector
        {
            get; private set;
        }

        public EndRoadLaneEdge GetOppositeEdge()
        {
            if ( this._parrent.LeftEdge == this )
            {
                return this._parrent.RightEdge;
            }

            // else
            return this._parrent.LeftEdge;
        }

        private void Contract( MovablePoint point )
        {
            System.Diagnostics.Contracts.Contract.Requires( point != null );
        }

        public void RecalculatePosition()
        {
            this.Contract( null );
            this.Invalidate();
            // TODO Implement
//            throw new NotImplementedException();
        }
    }
}
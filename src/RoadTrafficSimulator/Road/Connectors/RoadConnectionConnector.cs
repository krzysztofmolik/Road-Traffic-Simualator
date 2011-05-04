using System;
using RoadTrafficSimulator.Road.Controls;

namespace RoadTrafficSimulator.Road.Connectors
{
    public class RoadConnectionConnector
    {
        private readonly RoadConnection _owner;
        private readonly ConnectEdgesHelper _helper;

        public RoadConnectionConnector( RoadConnection owner )
        {
            this._owner = owner;
            this._helper = new ConnectEdgesHelper( owner );
        }

        public EndRoadLaneEdge PreviousEdge { get; private set; }

        public EndRoadLaneEdge NextEdge { get; private set; }

        public IEdgeLine Top { get; private set; }

        public IEdgeLine Bottom { get; private set; }

        public void ConnectBeginWith( EndRoadLaneEdge roadLaneEdge )
        {
            // TODO Check it
            this.PreviousEdge = this.GetLaneEdgeOpositeTo( roadLaneEdge );
            this.PreviousEdge.Translated.Subscribe( x => this._owner.RecalculatePosition() );

            this._owner.RecalculatePosition();
        }

        public void ConnectEndWith( EndRoadLaneEdge roadLaneEdge )
        {
            var otherSideOfLane = this.GetLaneEdgeOpositeTo( roadLaneEdge );
            this.NextEdge = otherSideOfLane;

            this.NextEdge.Translated.Subscribe( x => this._owner.RecalculatePosition() );
            this._owner.RecalculatePosition();
        }

        private EndRoadLaneEdge GetLaneEdgeOpositeTo( EndRoadLaneEdge roadLaneEdge )
        {
            var owner = roadLaneEdge.RoadLaneBlockParent;
            return owner.LeftEdge == roadLaneEdge ? owner.RightEdge : owner.LeftEdge;
        }

        public void ConnectBeginBottomWith( IEdgeLine roadConnection )
        {
            this.Bottom = roadConnection;
            this._helper.ConnectBeginBottomWith( roadConnection );
        }

        public void ConnectEndTopWith( IEdgeLine roadConnection )
        {
            this.Top = roadConnection;
            this._helper.ConnectEndTopWith( roadConnection );
        }

        public void ConnectBeginTopWith( IEdgeLine roadConnection )
        {
            this.Top = roadConnection;
            this._helper.ConnectBeginTopWith( roadConnection );
        }

        public void ConnectEndBottomWith( IEdgeLine roadConnection )
        {
            this.Bottom = roadConnection;
            this._helper.ConnectEndBottomWith( roadConnection );
        }
    }
}
using System;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using RoadTrafficSimulator.Infrastructure.Controls;

namespace RoadTrafficSimulator.Components.BuildMode.Connectors
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

        public EndRoadLaneEdge OpositeToPreviousEdge { get; private set; }
        public EndRoadLaneEdge PreviousConnectedEdge { get; private set; }

        // TODO Change this stupid names
        public EndRoadLaneEdge OpositeToNextEdge { get; private set; }
        public EndRoadLaneEdge NextConnectedEdge { get; private set; }

        public IEdgeLine Top { get; private set; }

        public IEdgeLine Bottom { get; private set; }

        public void ConnectBeginWith( EndRoadLaneEdge roadLaneEdge )
        {
            // TODO Check it
            this.PreviousConnectedEdge = roadLaneEdge;
            this.OpositeToPreviousEdge = this.GetLaneEdgeOpositeTo( roadLaneEdge );
            this.OpositeToPreviousEdge.Translated.Subscribe( x => this._owner.RecalculatePosition() );

            this._owner.RecalculatePosition();
        }

        public void ConnectEndWith( EndRoadLaneEdge roadLaneEdge )
        {
            this.NextConnectedEdge = roadLaneEdge;
            var otherSideOfLane = this.GetLaneEdgeOpositeTo( roadLaneEdge );
            this.OpositeToNextEdge = otherSideOfLane;

            this.OpositeToNextEdge.Translated.Subscribe( x => this._owner.RecalculatePosition() );
            this._owner.RecalculatePosition();
            this._owner.Routes.AddRoute( new RouteElement( roadLaneEdge.RoadLaneBlockParent, PriorityType.None ) );
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
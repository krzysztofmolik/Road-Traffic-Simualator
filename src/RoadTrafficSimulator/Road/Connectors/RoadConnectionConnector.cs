using System;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Road.Controls;
using XnaRoadTrafficConstructor.Road;
using XnaVs10.MathHelpers;

namespace RoadTrafficSimulator.Road.Connectors
{
    public class RoadConnectionConnector : ConnectorBase
    {
        private readonly RoadConnection _owner;

        public RoadConnectionConnector( RoadConnection owner )
        {
            this._owner = owner;
        }

        public EndRoadLaneEdge PreviousEdge { get; private set; }

        public EndRoadLaneEdge NextEdge { get; private set; }

        public RoadConnection Top { get; private set; }

        public RoadConnection Bottom { get; private set; }

        public void ConnectBeginWith( EndRoadLaneEdge roadLaneEdge )
        {
            // TODO Check it
            this.PreviousEdge = this.GetLaneEdgeOpositeTo( roadLaneEdge );
            this.ConnectBySubscribingToEvent( roadLaneEdge.StartPoint, this._owner.LeftEdge.EndPoint );
            this.ConnectBySubscribingToEvent( roadLaneEdge.EndPoint, this._owner.LeftEdge.StartPoint );
            this.PreviousEdge.Translated.Subscribe( x => this._owner.RecalculatePosition( this._owner.LeftEdge ) );

            this._owner.RecalculatePosition( this._owner.LeftEdge );
        }

        public void ConnectEndWith( EndRoadLaneEdge roadLaneEdge )
        {
            var otherSideOfLane = this.GetLaneEdgeOpositeTo( roadLaneEdge );
            this.NextEdge = otherSideOfLane;

            this.NextEdge.Translated.Subscribe( x => this._owner.RecalculatePosition( this._owner.RightEdge ) );
            this.ConnectBySubscribingToEvent( this._owner.RightEdge.StartPoint, roadLaneEdge.EndPoint );
            this.ConnectBySubscribingToEvent( this._owner.RightEdge.EndPoint, roadLaneEdge.StartPoint );
            this._owner.RecalculatePosition( this._owner.RightEdge );
        }

        private EndRoadLaneEdge GetLaneEdgeOpositeTo( EndRoadLaneEdge roadLaneEdge )
        {
            var owner = roadLaneEdge.RoadLaneBlockParent;
            return owner.LeftEdge == roadLaneEdge ? owner.RightEdge : owner.LeftEdge;
        }

        public void NotifyAboutTranslation()
        {
            if ( this.PreviousEdge != null )
            {
                this.PreviousEdge.RecalculatePosition();
            }

            if ( this.NextEdge != null )
            {
                this.NextEdge.RecalculatePosition();
            }
        }

        public void ConnectBeginBottomWith( RoadConnection roadConnection )
        {
            this.Bottom = roadConnection;
            this.ConnectBySubscribingToEvent( roadConnection.EndPoint, this._owner.StartPoint );
            this._owner.EndPoint.SetLocation( roadConnection.EndLocation );
        }

        public void ConnectEndTopWith( RoadConnection roadConnection )
        {
            this.Top = roadConnection;
            this.ConnectBySubscribingToEvent( roadConnection.StartPoint, this._owner.EndPoint );
        }

        public void ConnectBeginTopWith( RoadConnection roadConnection )
        {
            this.Top = roadConnection;
            this.ConnectBySubscribingToEvent( roadConnection.EndPoint, this._owner.StartPoint );
        }

        public void ConnectEndBottomWith( RoadConnection roadConnection )
        {
            this.Bottom = roadConnection;
            this.ConnectBySubscribingToEvent( roadConnection.StartPoint, this._owner.EndPoint );
            this._owner.EndPoint.SetLocation( roadConnection.StartLocation );
        }
    }
}
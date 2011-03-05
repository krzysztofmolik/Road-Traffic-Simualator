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

        public void ConnectBeginWith( EndRoadLaneEdge roadLaneEdge )
        {
            // TODO Check it
            this.PreviousEdge = this.GetLaneEdgeOpositeTo( roadLaneEdge );
            this.ConnectBySubscribingToEvent( roadLaneEdge.StartPoint, this._owner.EndPoint );
            this.ConnectBySubscribingToEvent( roadLaneEdge.EndPoint, this._owner.StartPoint );
            this.PreviousEdge.Translated.Subscribe( x => this._owner.RecalculatePosition() );

            this._owner.RecalculatePosition();
        }

        public void ConnectEndWith( EndRoadLaneEdge roadLaneEdge )
        {
            var otherSideOfLane = this.GetLaneEdgeOpositeTo( roadLaneEdge );
            this.NextEdge = otherSideOfLane;

            this.NextEdge.Translated.Subscribe( x => this._owner.RecalculatePosition() );
            this.ConnectBySubscribingToEvent( this._owner.StartPoint, roadLaneEdge.EndPoint );
            this.ConnectBySubscribingToEvent( this._owner.EndPoint, roadLaneEdge.StartPoint );
            this._owner.RecalculatePosition();
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
    }
}
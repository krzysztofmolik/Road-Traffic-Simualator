using System;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Road.Controls;
using XnaRoadTrafficConstructor.Road;
using XnaVs10.MathHelpers;
using XnaVs10.Extension;

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

        public void ConnectBeginBottomWith( RoadConnection roadConnection )
        {
            this.Bottom = roadConnection;
            roadConnection.EndPoint.Translated.Subscribe( _ =>
                                                             {
                                                                 var changed = this._owner.StartPoint.SetLocation( roadConnection.EndLocation );
                                                                 if ( changed ) { this._owner.RecalculatePostitionAroundStartPoint(); }
                                                             } );
            var delta = roadConnection.EndLocation - this._owner.StartLocation;
            this._owner.StartPoint.Translate( Matrix.CreateTranslation( delta.ToVector3() ) );
            this._owner.EndPoint.Translate( Matrix.CreateTranslation( delta.ToVector3() ) );
        }

        public void ConnectEndTopWith( RoadConnection roadConnection )
        {
            this.Top = roadConnection;
            roadConnection.StartPoint.Translated.Subscribe( _ =>
                                                               {
                                                                   var changed = this._owner.EndPoint.SetLocation( roadConnection.StartLocation );
                                                                   if ( changed ) { this._owner.RecalculatePostitionAroundEndPoint(); }
                                                               } );
        }

        public void ConnectBeginTopWith( RoadConnection roadConnection )
        {
            this.Top = roadConnection;
            roadConnection.StartPoint.Translated.Subscribe( _ =>
                                                               {
                                                                   var changed = this._owner.EndPoint.SetLocation( roadConnection.StartLocation );
                                                                   if ( changed ) { this._owner.RecalculatePostitionAroundEndPoint(); }
                                                               } );

            var delta = roadConnection.StartLocation - this._owner.EndLocation;
            this._owner.StartPoint.Translate( Matrix.CreateTranslation( delta.ToVector3() ) );
            this._owner.EndPoint.Translate( Matrix.CreateTranslation( delta.ToVector3() ) );
        }

        public void ConnectEndBottomWith( RoadConnection roadConnection )
        {
            this.Bottom = roadConnection;
            roadConnection.EndPoint.Translated.Subscribe( _ =>
                                                             {
                                                                 var changed = this._owner.StartPoint.SetLocation( roadConnection.EndLocation );
                                                                 if ( changed ) { this._owner.RecalculatePostitionAroundStartPoint(); }
                                                             } );
        }
    }
}
using System;
using Microsoft.Xna.Framework;
using XnaVs10.Extension;

namespace RoadTrafficSimulator.Road.Controls
{
    // TODO Remove duplication... if i have some free time 
    public class CarsInsertedConnector
    {
        private CarsInserter _owner;
        public CarsInsertedConnector( CarsInserter owner )
        {
            this._owner = owner;
        }

        public EndRoadLaneEdge ConnectedRoad { get; private set; }

        public CarsInserter Top { get; private set; }

        public CarsInserter Bottom { get; private set; }

        public void ConnectBeginWith( EndRoadLaneEdge roadLaneEdge )
        {
            this.ConnectedRoad = roadLaneEdge.GetOppositeEdge();
            this.ConnectedRoad.Translated.Subscribe( x => this._owner.RecalculatePosition() );

            this._owner.RecalculatePosition();
        }

        public void ConnectBeginBottomWith( CarsInserter roadConnection )
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

        public void ConnectEndTopWith( CarsInserter roadConnection )
        {
            this.Top = roadConnection;
            roadConnection.StartPoint.Translated.Subscribe( _ =>
                                                               {
                                                                   var changed = this._owner.EndPoint.SetLocation( roadConnection.StartLocation );
                                                                   if ( changed ) { this._owner.RecalculatePostitionAroundEndPoint(); }
                                                               } );
        }

        public void ConnectBeginTopWith( CarsInserter roadConnection )
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

        public void ConnectEndBottomWith( CarsInserter roadConnection )
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
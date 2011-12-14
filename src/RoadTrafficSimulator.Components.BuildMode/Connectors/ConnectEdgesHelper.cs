using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using System;
using RoadTrafficSimulator.Infrastructure.Extension;

namespace RoadTrafficSimulator.Components.BuildMode.Connectors
{
    public class ConnectEdgesHelper
    {
        private readonly IEdge _edgeOwner;
        private readonly IEdgeLine _edgeLineOwner;

        public ConnectEdgesHelper( IEdge edgeOwner, IEdgeLine edgeLineOwner )
        {
            this._edgeOwner = edgeOwner;
            this._edgeLineOwner = edgeLineOwner;
        }

        public void ConnectBeginBottomWith( Edge roadConnection )
        {
            roadConnection.EndPoint.Translated.Subscribe( _ =>
                                                              {
                                                                  var changed = this._edgeOwner.StartPoint.SetLocation( roadConnection.EndPoint.Location );
                                                                  if ( changed ) { this._edgeLineOwner.RecalculatePostitionAroundStartPoint(); }
                                                              } );
            var delta = roadConnection.EndPoint.Location - this._edgeOwner.StartPoint.Location;
            this._edgeOwner.StartPoint.Translate( Matrix.CreateTranslation( delta.ToVector3() ) );
            this._edgeOwner.EndPoint.Translate( Matrix.CreateTranslation( delta.ToVector3() ) );
        }

        public void ConnectEndTopWith( Edge roadConnection )
        {
            roadConnection.StartPoint.Translated.Subscribe( _ =>
                                                                {
                                                                    var changed = this._edgeOwner.EndPoint.SetLocation( roadConnection.StartPoint.Location );
                                                                    if ( changed ) { this._edgeLineOwner.RecalculatePostitionAroundEndPoint(); }
                                                                } );
        }

        public void ConnectBeginTopWith( Edge roadConnection )
        {
            roadConnection.StartPoint.Translated.Subscribe( _ =>
                                                                {
                                                                    var changed = this._edgeOwner.EndPoint.SetLocation( roadConnection.StartPoint.Location );
                                                                    if ( changed ) { this._edgeLineOwner.RecalculatePostitionAroundEndPoint(); }
                                                                } );

            var delta = roadConnection.StartPoint.Location - this._edgeOwner.EndPoint.Location;
            this._edgeOwner.StartPoint.Translate( Matrix.CreateTranslation( delta.ToVector3() ) );
            this._edgeOwner.EndPoint.Translate( Matrix.CreateTranslation( delta.ToVector3() ) );
        }

        public void ConnectEndBottomWith( Edge roadConnection )
        {
            roadConnection.EndPoint.Translated.Subscribe( _ =>
                                                              {
                                                                  var changed = this._edgeOwner.StartPoint.SetLocation( roadConnection.EndPoint.Location );
                                                                  if ( changed ) { this._edgeLineOwner.RecalculatePostitionAroundStartPoint(); }
                                                              } );
        }
    }
}
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Components.BuildMode.Controls;
using System;
using RoadTrafficSimulator.Infrastructure.Extension;

namespace RoadTrafficSimulator.Components.BuildMode.Connectors
{
    public class ConnectEdgesHelper
    {
        private readonly IEdgeLine _owner;

        public ConnectEdgesHelper( IEdgeLine owner )
        {
            Contract.Requires( owner != null );
            this._owner = owner;
        }

        public void ConnectBeginBottomWith( IEdgeLine roadConnection )
        {
            roadConnection.EndPoint.Translated.Subscribe( _ =>
                                                              {
                                                                  var changed = this._owner.StartPoint.SetLocation( roadConnection.EndPoint.Location );
                                                                  if ( changed ) { this._owner.RecalculatePostitionAroundStartPoint(); }
                                                              } );
            var delta = roadConnection.EndPoint.Location - this._owner.StartPoint.Location;
            this._owner.StartPoint.Translate( Matrix.CreateTranslation( delta.ToVector3() ) );
            this._owner.EndPoint.Translate( Matrix.CreateTranslation( delta.ToVector3() ) );
        }

        public void ConnectEndTopWith( IEdgeLine roadConnection )
        {
            roadConnection.StartPoint.Translated.Subscribe( _ =>
                                                                {
                                                                    var changed = this._owner.EndPoint.SetLocation( roadConnection.StartPoint.Location );
                                                                    if ( changed ) { this._owner.RecalculatePostitionAroundEndPoint(); }
                                                                } );
        }

        public void ConnectBeginTopWith( IEdgeLine roadConnection )
        {
            roadConnection.StartPoint.Translated.Subscribe( _ =>
                                                                {
                                                                    var changed = this._owner.EndPoint.SetLocation( roadConnection.StartPoint.Location );
                                                                    if ( changed ) { this._owner.RecalculatePostitionAroundEndPoint(); }
                                                                } );

            var delta = roadConnection.StartPoint.Location - this._owner.EndPoint.Location;
            this._owner.StartPoint.Translate( Matrix.CreateTranslation( delta.ToVector3() ) );
            this._owner.EndPoint.Translate( Matrix.CreateTranslation( delta.ToVector3() ) );
        }

        public void ConnectEndBottomWith( IEdgeLine roadConnection )
        {
            roadConnection.EndPoint.Translated.Subscribe( _ =>
                                                              {
                                                                  var changed = this._owner.StartPoint.SetLocation( roadConnection.EndPoint.Location );
                                                                  if ( changed ) { this._owner.RecalculatePostitionAroundStartPoint(); }
                                                              } );
        }
    }
}
using System;
using System.Diagnostics.Contracts;
using RoadTrafficSimulator.Components.BuildMode.Connectors;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class CarsRemoverConnector
    {
        private readonly CarsRemover _owner;
        private readonly ConnectEdgesHelper _connectEdgesHelper;

        public CarsRemoverConnector( CarsRemover owner )
        {
            Contract.Requires( owner != null );
            this._owner = owner;
            this._connectEdgesHelper = new ConnectEdgesHelper( this._owner.Edge, this._owner );
        }

        public EndRoadLaneEdge OpositeEdge { get; private set; }
        public EndRoadLaneEdge ConnectedEdge { get; private set; }

        public CarsRemover Top { get; private set; }

        public CarsRemover Bottom { get; private set; }

        public void ConnectEndOn( RoadLaneBlock roadLaneEdge )
        {
            this.ConnectedEdge = roadLaneEdge.RightEdge;
            this.OpositeEdge = roadLaneEdge.RightEdge.GetOppositeEdge();
            this.OpositeEdge.Translated.Subscribe( x => this._owner.RecalculatePosition() );

            this._owner.RecalculatePosition();
        }

        public void ConnectBeginBottomWith( CarsRemover roadConnection )
        {
            this.Bottom = roadConnection;
            this._connectEdgesHelper.ConnectBeginBottomWith( roadConnection.Edge );
        }

        public void ConnectEndTopWith( CarsRemover roadConnection )
        {
            this.Top = roadConnection;
            this._connectEdgesHelper.ConnectEndTopWith( roadConnection.Edge );
        }

        public void ConnectBeginTopWith( CarsRemover roadConnection )
        {
            this.Top = roadConnection;
            this._connectEdgesHelper.ConnectBeginTopWith( roadConnection.Edge );
        }

        public void ConnectEndBottomWith( CarsRemover roadConnection )
        {
            this.Bottom = roadConnection;
            this._connectEdgesHelper.ConnectEndBottomWith( roadConnection.Edge );
        }

    }
}
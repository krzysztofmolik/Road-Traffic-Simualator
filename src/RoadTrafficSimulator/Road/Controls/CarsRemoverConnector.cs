using System;
using System.Diagnostics.Contracts;
using RoadTrafficSimulator.Road.Connectors;

namespace RoadTrafficSimulator.Road.Controls
{
    public class CarsRemoverConnector
    {
        private readonly CarsRemover _owner;
        private readonly ConnectEdgesHelper _connectEdgesHelper;

        public CarsRemoverConnector( CarsRemover owner )
        {
            Contract.Requires( owner != null );
            this._owner = owner;
            this._connectEdgesHelper = new ConnectEdgesHelper( owner );
        }

        public EndRoadLaneEdge ConnectedRoad { get; private set; }

        public CarsRemover Top { get; private set; }

        public CarsRemover Bottom { get; private set; }

        public void ConnectBeginWith( EndRoadLaneEdge roadLaneEdge )
        {
            this.ConnectedRoad = roadLaneEdge.GetOppositeEdge();
            this.ConnectedRoad.Translated.Subscribe( x => this._owner.RecalculatePosition() );

            this._owner.RecalculatePosition();
        }

        public void ConnectBeginBottomWith( CarsRemover roadConnection )
        {
            this.Bottom = roadConnection;
            this._connectEdgesHelper.ConnectBeginBottomWith( roadConnection );
        }

        public void ConnectEndTopWith( CarsRemover roadConnection )
        {
            this.Top = roadConnection;
            this._connectEdgesHelper.ConnectEndTopWith( roadConnection );
        }

        public void ConnectBeginTopWith( CarsRemover roadConnection )
        {
            this.Top = roadConnection;
            this._connectEdgesHelper.ConnectBeginTopWith( roadConnection );
        }

        public void ConnectEndBottomWith( CarsRemover roadConnection )
        {
            this.Bottom = roadConnection;
            this._connectEdgesHelper.ConnectEndBottomWith( roadConnection );
        }

    }
}
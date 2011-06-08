using System;
using System.Diagnostics.Contracts;
using RoadTrafficSimulator.Components.BuildMode.Connectors;

namespace RoadTrafficSimulator.Components.BuildMode.Controls
{
    public class CarsInsertedConnector
    {
        private readonly CarsInserter _owner;
        private readonly ConnectEdgesHelper _connectEdgesHelper;

        public CarsInsertedConnector( CarsInserter owner )
        {
            Contract.Requires( owner != null );
            this._owner = owner;
            this._connectEdgesHelper = new ConnectEdgesHelper( owner );
        }

        public EndRoadLaneEdge ConnectedRoad { get; private set; }

        public CarsInserter Top { get; private set; }

        public CarsInserter Bottom { get; private set; }

        public void ConnectEndWith( EndRoadLaneEdge roadLaneEdge )
        {
            this.ConnectedRoad = roadLaneEdge.GetOppositeEdge();
            this.ConnectedRoad.Translated.Subscribe( x => this._owner.RecalculatePosition() );

            this._owner.RecalculatePosition();
        }

        public void ConnectBeginBottomWith( CarsInserter roadConnection )
        {
            this.Bottom = roadConnection;
            this._connectEdgesHelper.ConnectBeginBottomWith( roadConnection );
        }

        public void ConnectEndTopWith( CarsInserter roadConnection )
        {
            this.Top = roadConnection;
            this._connectEdgesHelper.ConnectEndTopWith( roadConnection );
        }

        public void ConnectBeginTopWith( CarsInserter roadConnection )
        {
            this.Top = roadConnection;
            this._connectEdgesHelper.ConnectBeginTopWith( roadConnection );
        }

        public void ConnectEndBottomWith( CarsInserter roadConnection )
        {
            this.Bottom = roadConnection;
            this._connectEdgesHelper.ConnectEndBottomWith( roadConnection );
        }

    }
}
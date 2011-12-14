using System;
using System.Diagnostics.Contracts;
using RoadTrafficSimulator.Components.BuildMode.Connectors;
using RoadTrafficSimulator.Infrastructure.Controls;

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
            this._connectEdgesHelper = new ConnectEdgesHelper(this._owner.Edge, owner );
        }

        public EndRoadLaneEdge OpositeEdge { get; private set; }
        public EndRoadLaneEdge ConnectedEdge { get; private set; }

        public CarsInserter Top { get; private set; }

        public CarsInserter Bottom { get; private set; }

        public void ConnectStartFrom( RoadLaneBlock roadLaneEdge )
        {
            this.ConnectedEdge = roadLaneEdge.LeftEdge;
            this.OpositeEdge = roadLaneEdge.LeftEdge.GetOppositeEdge();
            this.OpositeEdge.Translated.Subscribe( x => this._owner.RecalculatePosition() );
            this._owner.RecalculatePosition();
            this._owner.Routes.AddRoute( new RouteElement( roadLaneEdge, PriorityType.None ) );
        }

        public void ConnectBeginBottomWith( CarsInserter roadConnection )
        {
            this.Bottom = roadConnection;
            this._connectEdgesHelper.ConnectBeginBottomWith( roadConnection.Edge );
        }

        public void ConnectEndTopWith( CarsInserter roadConnection )
        {
            this.Top = roadConnection;
            this._connectEdgesHelper.ConnectEndTopWith( roadConnection.Edge );
        }

        public void ConnectBeginTopWith( CarsInserter roadConnection )
        {
            this.Top = roadConnection;
            this._connectEdgesHelper.ConnectBeginTopWith( roadConnection.Edge );
        }

        public void ConnectEndBottomWith( CarsInserter roadConnection )
        {
            this.Bottom = roadConnection;
            this._connectEdgesHelper.ConnectEndBottomWith( roadConnection.Edge );
        }

    }
}
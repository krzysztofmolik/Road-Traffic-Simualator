using System;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Road.Controls;

namespace RoadTrafficSimulator.Road.Connectors
{
    public class SideRoadLaneConnector : ConnectorBase
    {
        private readonly SideRoadLaneEdge _owner;

        public SideRoadLaneConnector( SideRoadLaneEdge owner )
        {
            this._owner = owner;
        }

        public SideRoadLaneEdge SideRoadLaneEdge { get; private set; }

        // TODO Change name
        public void ConnectChangeName( SideRoadLaneEdge edge )
        {
            this.ConnectBySubscribingToEvent( this._owner, edge );
            this.SideRoadLaneEdge = edge;
        }
    }
}
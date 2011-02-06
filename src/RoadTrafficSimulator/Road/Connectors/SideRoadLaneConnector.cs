using System;
using RoadTrafficSimulator.Infrastructure.Mouse;
using RoadTrafficSimulator.Road.Controls;

namespace RoadTrafficSimulator.Road.Connectors
{
    public class SideRoadLaneConnector : ConnectorBase
    {
        private const int MAX_CONNECTED_OBJECT = 1;

        private readonly SideRoadLaneEdge _owner;

        public SideRoadLaneConnector( SideRoadLaneEdge owner )
            : base( MAX_CONNECTED_OBJECT )
        {
            this._owner = owner;
        }

        public void ConnectTo( SideRoadLaneEdge otherEdge )
        {
            this.ConnectBySubscribingToEvent( this._owner, otherEdge );
            this.AddConnectedObject( otherEdge );
        }
    }
}
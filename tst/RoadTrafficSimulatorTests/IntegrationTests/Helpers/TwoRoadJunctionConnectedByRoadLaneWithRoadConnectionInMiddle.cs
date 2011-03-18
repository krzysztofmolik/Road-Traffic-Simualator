using Microsoft.Xna.Framework;
using RoadTrafficSimulator.Factories;
using RoadTrafficSimulator.Road;
using RoadTrafficSimulator.Road.Connectors.Commands;
using RoadTrafficSimulator.Road.Controls;
using RoadTrafficSimulatorTests.IntegrationTests.Infacstructure;

namespace RoadTrafficSimulatorTests.IntegrationTests.Helpers
{
    public class TwoRoadJunctionConnectedByRoadLaneWithRoadConnectionInMiddle
    {
        public RoadJunctionBlock FirstRoadJunction { get; private set; }
        public RoadJunctionBlock SecondRoadJunction { get; private set; }
        public RoadLaneBlock FirstRoadLaneBlock { get; private set; }
        public RoadLaneBlock SecondRoadLaneBlock { get; private set; }
        public RoadConnection RoadConnection { get; private set; }

        public TwoRoadJunctionConnectedByRoadLaneWithRoadConnectionInMiddle()
        {
            var factories = IOC.GetService<Factories>();
            this.FirstRoadJunction = new RoadJunctionBlock( factories, Vector2.Zero, null );
            this.SecondRoadJunction = new RoadJunctionBlock( factories, new Vector2( 2.0f, 0 ), null );
            this.FirstRoadLaneBlock = new RoadLaneBlock( factories, null );
            this.SecondRoadLaneBlock = new RoadLaneBlock( factories, null );
            this.RoadConnection = new RoadConnection( factories, new Vector2( 1.0f, 0.0f ), null );
        }

        public void Connect()
        {
            var roadJunctionWithRoadLaneConnection = IOC.GetService<ConnectRoadJunctionEdgeWitEndRoadLaneEdge>();
            var roadLaneWithRoadConnection = IOC.GetService<ConnectEndRoadLaneEdgeWithRoadLaneConnection>();
            var roadConnectionWithRoadLane = IOC.GetService<ConnectRoadConnectionWithEndRoadLane>();
            var roadLaneWithRoadJunction = IOC.GetService<ConnectEndRoadLaneEdgeWithRoadJunctionEdge>();

            roadJunctionWithRoadLaneConnection.Connect( this.FirstRoadJunction, this.FirstRoadLaneBlock );
            roadLaneWithRoadConnection.Connect( this.FirstRoadLaneBlock, this.RoadConnection );
            roadConnectionWithRoadLane.Connect( this.RoadConnection, this.SecondRoadLaneBlock );
            roadLaneWithRoadJunction.Connect( this.SecondRoadLaneBlock, this.SecondRoadJunction):
        }
    }
}
using Microsoft.Xna.Framework;
using NUnit.Framework;
using RoadTrafficSimulator.Road.Connectors.Commands;
using RoadTrafficSimulatorTests.IntegrationTests.Helpers;
using RoadTrafficSimulator.Extension;
using RoadTrafficSimulatorTests.IntegrationTests.Infacstructure;

namespace RoadTrafficSimulatorTests.ConnectionTests
{
    [TestFixture]
    public class ConnectedTwoRoadConnectionAndMove
    {
        private TwoRoadJunctionConnectedByRoadLaneWithRoadConnectionInMiddle _firstLane;
        private TwoRoadJunctionConnectedByRoadLaneWithRoadConnectionInMiddle _secondLane;
        private ConnectRoadConnectionWithRoadConnection _roadConnectionConnectionCommand;

        [SetUp]
        public void SetUp()
        {
            this._roadConnectionConnectionCommand = IOC.GetService<ConnectRoadConnectionWithRoadConnection>();
            this._firstLane = new TwoRoadJunctionConnectedByRoadLaneWithRoadConnectionInMiddle();
            this._secondLane = new TwoRoadJunctionConnectedByRoadLaneWithRoadConnectionInMiddle();
            this._secondLane.FirstRoadJunction.SetLocation( new Vector2( 0.0f, -0.1f ) );
            this._secondLane.RoadConnection.SetLocation( new Vector2( 1.0f, -0.1f ) );
            this._secondLane.SecondRoadJunction.SetLocation( new Vector2( 2.0f, -0.1f ) );
        }

        [Test]
        public void TestChangeName()
        {
            this._roadConnectionConnectionCommand.Connect( this._firstLane.RoadConnection, this._secondLane.RoadConnection );
            this._firstLane.SecondRoadJunction.SetLocation( new Vector2( 2.0f, 2.0f ) );
        }
    }
}